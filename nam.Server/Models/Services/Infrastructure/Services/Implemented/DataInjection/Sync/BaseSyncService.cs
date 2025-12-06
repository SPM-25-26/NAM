using EFCore.BulkExtensions;
using nam.Server.Data;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;
using System.Collections.Concurrent;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Sync
{
    public abstract class BaseSyncService<TDto, TEntity>(ApplicationDbContext dbContext, Serilog.ILogger logger, IConfiguration Configuration) : ISyncService
        where TEntity : class
    {
        protected readonly ApplicationDbContext _dbContext = dbContext;
        protected readonly Serilog.ILogger _logger = logger;
        private readonly IConfiguration _configuration = Configuration;

        public async Task ExecuteSyncAsync()
        {
            _logger.Information($"Starting sync of {typeof(TEntity).Name}...");

            var municipalities = _configuration.GetSection("Municipalities").Get<string[]>() ?? [];
            var allEntities = new ConcurrentBag<TEntity>();

            // Fetch in parallelo
            await Parallel.ForEachAsync(municipalities, async (municipality, ct) =>
            {
                _logger.Information($"Fetching data for municipality: {municipality}");

                var entities = await GetEntities(municipality);

                if (entities == null || entities.Count == 0)
                {
                    _logger.Warning($"No data fetched for municipality: {municipality}");
                    return;
                }

                foreach (var entity in entities)
                {
                    allEntities.Add(entity);
                }

                _logger.Information($"Fetched {entities.Count} records for {municipality}");
            });

            if (allEntities.Count == 0)
            {
                _logger.Warning("No data to sync");
                return;
            }

            // Bulk operation unica per tutti i dati
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            await _dbContext.BulkInsertOrUpdateAsync(allEntities.ToList(), new BulkConfig { IncludeGraph = true });
            await transaction.CommitAsync();

            _logger.Information($"Sync completed. Total processed: {allEntities.Count} records across {municipalities.Length} municipalities.");
        }

        protected abstract Task<List<TEntity>> GetEntities(string municipality);
    }
}