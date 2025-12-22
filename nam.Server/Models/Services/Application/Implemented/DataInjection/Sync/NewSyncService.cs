using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using nam.Server.Data;
using nam.Server.Models.Services.Application.Interfaces.DataInjection;
using System.Collections.Concurrent;
using System.Reflection;

namespace nam.Server.Models.Services.Application.Implemented.DataInjection.Sync
{
    public class NewSyncService(ApplicationDbContext dbContext, Serilog.ILogger logger, IConfiguration Configuration) : ISyncService
    {
        protected readonly ApplicationDbContext _dbContext = dbContext;
        protected readonly Serilog.ILogger _logger = logger;
        private readonly IConfiguration _configuration = Configuration;

        /// <summary>
        /// Executes the synchronization process for a specific entity type.
        /// Fetches data from external sources, cleans up existing related data, and performs a bulk insert/update.
        /// </summary>
        public async Task ExecuteSyncAsync<TEntity>(IEntityCollector<TEntity> entityCollector) where TEntity : class
        {
            _logger.Information("Starting synchronization process for entity: {EntityName}", typeof(TEntity).Name);

            var municipalities = _configuration.GetSection("Municipalities").Get<string[]>() ?? [];
            var allEntities = new ConcurrentBag<TEntity>();

            // 1. DATA FETCHING
            // Retrieve entities from all configured municipalities in parallel.
            await Parallel.ForEachAsync(municipalities, async (municipality, ct) =>
            {
                try
                {
                    var entities = await entityCollector.GetEntities(municipality);
                    if (entities != null)
                    {
                        foreach (var e in entities) allEntities.Add(e);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("Error fetching data for municipality {Municipality}: {ErrorMessage}", municipality, ex.Message);
                }
            });

            if (allEntities.IsEmpty)
            {
                _logger.Warning("No entities were fetched. Aborting synchronization.");
                return;
            }

            // 2. PRIMARY KEY EXTRACTION
            // Identify the Primary Key property to track parent IDs for cleanup.
            var entityType = _dbContext.Model.FindEntityType(typeof(TEntity));
            var primaryKey = entityType?.FindPrimaryKey();

            // Fallback strategy: Use the EF Core metadata PK, or look for a property named "Id".
            var pkPropInfo = primaryKey?.Properties.First().PropertyInfo
                             ?? typeof(TEntity).GetProperty(primaryKey?.Properties.First().Name ?? "Id");

            if (pkPropInfo == null)
            {
                _logger.Error("Fatal Error: Could not determine Primary Key for entity {EntityName}.", typeof(TEntity).Name);
                return;
            }

            var parentIds = new List<Guid>();
            foreach (var entity in allEntities)
            {
                var val = pkPropInfo.GetValue(entity);
                if (val is Guid g) parentIds.Add(g);
                else if (val != null && Guid.TryParse(val.ToString(), out Guid parsed)) parentIds.Add(parsed);
            }

            // 3. DATABASE TRANSACTION
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // A. CLEANUP PROCESS
                // Remove existing related child data to prevent duplicates or foreign key conflicts.
                await CleanRelatedDataAsync<TEntity>(parentIds);

                // B. BULK UPSERT
                // Insert new records or update existing ones using bulk operations for performance.
                await _dbContext.BulkInsertOrUpdateAsync(allEntities, new BulkConfig
                {
                    IncludeGraph = true,
                    BulkCopyTimeout = 600,
                    // Optional: Enable if generated identity columns need to be propagated back to the object.
                    // SetOutputIdentity = true 
                });

                await transaction.CommitAsync();
                _logger.Information("Synchronization completed successfully for entity: {EntityName}", typeof(TEntity).Name);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.Error(ex, "Synchronization failed. Transaction rolled back.");
                throw;
            }
        }

        /// <summary>
        /// Orchestrates the cleanup of child and grandchild entities based on the parent IDs.
        /// </summary>
        private async Task CleanRelatedDataAsync<TParent>(List<Guid> parentIds) where TParent : class
        {
            var parentType = typeof(TParent);

            // PHASE 1: PRE-CLEANING GRANDCHILDREN (Children of the "Detail" entity)
            // If the parent has a "Detail" property (1:1 relation), we must clean its children first.
            // Failing to do so might cause Foreign Key constraint violations when deleting the Detail entity itself.
            var detailProperty = parentType.GetProperty("Detail");
            if (detailProperty != null)
            {
                var detailType = detailProperty.PropertyType;
                _logger.Information("Pre-cleaning grandchild entities for Detail Type: {DetailTypeName}", detailType.Name);

                // Assumption: The Detail.Identifier matches the Parent.EntityId (enforced in the Collector).
                await DeleteChildrenOfEntity(detailType, parentIds);
            }

            // PHASE 2: CLEANING DIRECT CHILDREN
            // This includes the "Detail" entity itself and any other direct 1:N collections on the Parent.
            _logger.Information("Cleaning direct child entities of {ParentTypeName}", parentType.Name);
            await DeleteChildrenOfEntity(parentType, parentIds);
        }

        /// <summary>
        /// Identifies foreign keys pointing to the specified entity type and invokes the deletion logic.
        /// </summary>
        private async Task DeleteChildrenOfEntity(Type parentEntityType, List<Guid> parentIds)
        {
            var efEntityType = _dbContext.Model.FindEntityType(parentEntityType);
            if (efEntityType == null) return;

            // Retrieve all Foreign Keys where the Principal Entity is the current parentEntityType.
            var foreignKeys = _dbContext.Model.GetEntityTypes()
                .SelectMany(et => et.GetForeignKeys())
                .Where(fk => fk.PrincipalEntityType == efEntityType)
                .ToList();

            foreach (var fk in foreignKeys)
            {
                // Skip "Owned Types" as they are typically managed automatically by EF Core aggregates.
                if (fk.IsOwnership) continue;

                var childEntityType = fk.DeclaringEntityType;
                var childClrType = childEntityType.ClrType;
                var fkProperty = fk.Properties.FirstOrDefault();

                if (fkProperty == null) continue;

                // Identify the Foreign Key property name (even if it is a Shadow Property).
                string shadowPropertyName = fkProperty.Name;

                // Dynamically invoke the generic DeleteChildren method for the specific child type.
                var method = typeof(NewSyncService)
                    .GetMethod(nameof(DeleteChildren), BindingFlags.NonPublic | BindingFlags.Instance)!
                    .MakeGenericMethod(childClrType);

                await (Task)method.Invoke(this, new object[] { shadowPropertyName, parentIds })!;
            }
        }

        /// <summary>
        /// Performs the actual deletion of records using EF Core's ExecuteDeleteAsync.
        /// Uses batching to avoid exceeding SQL parameter limits.
        /// </summary>
        private async Task DeleteChildren<TChild>(string fkPropertyName, List<Guid> parentIds) where TChild : class
        {
            try
            {
                // Batch size to prevent SQL timeout or parameter limit errors.
                const int BatchSize = 2000;
                int totalDeleted = 0;

                for (int i = 0; i < parentIds.Count; i += BatchSize)
                {
                    var batch = parentIds.Skip(i).Take(BatchSize).ToList();

                    // Uses EF.Property to target the Foreign Key, handling Shadow Properties correctly.
                    var rows = await _dbContext.Set<TChild>()
                        .Where(x => batch.Contains(EF.Property<Guid>(x, fkPropertyName)))
                        .ExecuteDeleteAsync();

                    totalDeleted += rows;
                }

                if (totalDeleted > 0)
                {
                    _logger.Information("Successfully deleted {Count} records from {ChildType} (FK: {FkName})", totalDeleted, typeof(TChild).Name, fkPropertyName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to delete records for entity {ChildType}", typeof(TChild).Name);
            }
        }
    }
}