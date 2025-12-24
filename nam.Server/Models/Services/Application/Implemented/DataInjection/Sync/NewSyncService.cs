using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
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
        /// Entry point for synchronization.
        /// It inspects the entity to determine the Primary Key type, then invokes the strictly typed internal handler.
        /// </summary>
        public async Task ExecuteSyncAsync<TEntity>(IEntityCollector<TEntity> entityCollector) where TEntity : class
        {
            _logger.Information("Starting synchronization setup for entity: {EntityName}", typeof(TEntity).Name);

            // 1. Determine the Primary Key Type (Guid, String, Int, etc.)
            var entityType = _dbContext.Model.FindEntityType(typeof(TEntity));
            var primaryKey = entityType?.FindPrimaryKey();
            var pkProperty = primaryKey?.Properties.FirstOrDefault();

            if (pkProperty == null)
            {
                _logger.Error("Fatal Error: Could not determine Primary Key for entity {EntityName}.", typeof(TEntity).Name);
                return;
            }

            var pkType = pkProperty.ClrType; // e.g., typeof(Guid) or typeof(string)

            // 2. Dynamically invoke the generic internal method with the correct TKey
            // We use Reflection here to bridge the gap between TEntity and TEntity+TKey
            var method = typeof(NewSyncService)
                .GetMethod(nameof(ExecuteSyncInternalAsync), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(typeof(TEntity), pkType);

            await (Task)method.Invoke(this, new object[] { entityCollector, pkProperty.Name })!;
        }

        /// <summary>
        /// Internal method that handles the sync logic with knowledge of the Primary Key type (TKey).
        /// </summary>
        private async Task ExecuteSyncInternalAsync<TEntity, TKey>(IEntityCollector<TEntity> entityCollector, string pkPropertyName)
    where TEntity : class
    where TKey : notnull
        {
            _logger.Information("Executing synchronization for {EntityName} with Key Type: {KeyType}", typeof(TEntity).Name, typeof(TKey).Name);

            var municipalities = _configuration.GetSection("Municipalities").Get<string[]>() ?? [];
            var allEntities = new ConcurrentBag<TEntity>();

            // 1. DATA FETCHING
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

            // 2. PRIMARY KEY EXTRACTION (Type-Safe)
            var pkPropInfo = typeof(TEntity).GetProperty(pkPropertyName);
            if (pkPropInfo == null)
            {
                _logger.Error("Could not access property {PropertyName} on {EntityName}", pkPropertyName, typeof(TEntity).Name);
                return;
            }

            var parentIds = new List<TKey>();
            foreach (var entity in allEntities)
            {
                var val = pkPropInfo.GetValue(entity);
                if (val is TKey keyVal) parentIds.Add(keyVal);
            }

            // 3. DATABASE TRANSACTION CON EXECUTION STRATEGY
            // Creiamo la strategia di esecuzione definita nel DbContext (es. SqlServerRetryingExecutionStrategy)
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                // La transazione deve essere aperta ALL'INTERNO di ExecuteAsync
                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                try
                {
                    // A. CLEANUP PROCESS
                    await CleanRelatedDataAsync<TEntity, TKey>(parentIds);

                    // B. BULK UPSERT
                    await _dbContext.BulkInsertOrUpdateAsync(allEntities.ToList(), new BulkConfig
                    {
                        IncludeGraph = true,
                        BulkCopyTimeout = 600
                    });

                    await transaction.CommitAsync();
                    _logger.Information("Synchronization completed successfully for entity: {EntityName}", typeof(TEntity).Name);
                }
                catch (Exception ex)
                {
                    // Non è strettamente necessario il Rollback manuale qui perché il 'using' della transazione 
                    // o il fallimento dell'ExecuteAsync lo gestirebbero, ma è buona norma.
                    await transaction.RollbackAsync();
                    _logger.Error(ex, "Synchronization failed during transaction. Strategy will retry if transient.");
                    throw; // Rilancia l'eccezione per permettere alla strategia di fare il retry
                }
            });
        }

        /// <summary>
        /// Cleaning logic that accepts a generic Key Type.
        /// </summary>
        private async Task CleanRelatedDataAsync<TParent, TKey>(List<TKey> parentIds)
            where TParent : class
            where TKey : notnull
        {
            var parentType = typeof(TParent);

            // PHASE 1: PRE-CLEANING GRANDCHILDREN
            var detailProperty = parentType.GetProperty("Detail");
            if (detailProperty != null)
            {
                var detailType = detailProperty.PropertyType;
                _logger.Information("Pre-cleaning grandchild entities for Detail Type: {DetailTypeName}", detailType.Name);

                // We assume the Detail shares the same ID type and value as the Parent
                await DeleteChildrenOfEntity<TKey>(detailType, parentIds);
            }

            // PHASE 2: CLEANING DIRECT CHILDREN
            _logger.Information("Cleaning direct child entities of {ParentTypeName}", parentType.Name);
            await DeleteChildrenOfEntity<TKey>(parentType, parentIds);
        }

        /// <summary>
        /// Finds Foreign Keys pointing to the parent entity and triggers deletion.
        /// </summary>
        private async Task DeleteChildrenOfEntity<TKey>(Type parentEntityType, List<TKey> parentIds)
            where TKey : notnull
        {
            var efEntityType = _dbContext.Model.FindEntityType(parentEntityType);
            if (efEntityType == null) return;

            var foreignKeys = _dbContext.Model.GetEntityTypes()
                .SelectMany(et => et.GetForeignKeys())
                .Where(fk => fk.PrincipalEntityType == efEntityType)
                .ToList();

            foreach (var fk in foreignKeys)
            {
                if (fk.IsOwnership) continue;

                var childEntityType = fk.DeclaringEntityType;
                var childClrType = childEntityType.ClrType;
                var fkProperty = fk.Properties.FirstOrDefault();

                if (fkProperty == null) continue;

                string shadowPropertyName = fkProperty.Name;

                // Dynamically invoke DeleteChildren with the correct Child Type AND Key Type
                var method = typeof(NewSyncService)
                    .GetMethod(nameof(DeleteChildren), BindingFlags.NonPublic | BindingFlags.Instance)!
                    .MakeGenericMethod(childClrType, typeof(TKey));

                await (Task)method.Invoke(this, new object[] { shadowPropertyName, parentIds })!;
            }
        }

        /// <summary>
        /// Performs the delete using the generic TKey.
        /// </summary>
        private async Task DeleteChildren<TChild, TKey>(string fkPropertyName, List<TKey> parentIds)
            where TChild : class
            where TKey : notnull
        {
            try
            {
                const int BatchSize = 2000;
                int totalDeleted = 0;

                for (int i = 0; i < parentIds.Count; i += BatchSize)
                {
                    var batch = parentIds.Skip(i).Take(BatchSize).ToList();

                    // EF.Property<TKey> allows us to filter by String, Guid, Int, etc.
                    var rows = await _dbContext.Set<TChild>()
                        .Where(x => batch.Contains(EF.Property<TKey>(x, fkPropertyName)))
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