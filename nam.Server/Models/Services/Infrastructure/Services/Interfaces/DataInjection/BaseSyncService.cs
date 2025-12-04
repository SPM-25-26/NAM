using EFCore.BulkExtensions;
using nam.Server.Data;

namespace nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection
{
    public abstract class BaseSyncService<TDto, TEntity>(ApplicationDbContext dbContext, ILogger logger, IConfiguration Configuration, string municipality) : ISyncService
        where TEntity : class
    {
        protected readonly ApplicationDbContext _dbContext = dbContext;
        protected readonly ILogger _logger = logger;
        private readonly IConfiguration _configuration = Configuration;
        private readonly string _municipality = municipality;

        // IL TEMPLATE METHOD: Questo definisce l'algoritmo
        public async Task ExecuteSyncAsync()
        {
            _logger.LogInformation($"Starting sync of {typeof(TEntity).Name}...");

            // 1. Hook: Scaricamento dati (specifico per ogni classe figlia)
            var entities = await GetEntities(_municipality);

            if (entities == null || entities.Count == 0)
            {
                _logger.LogWarning("No data fetched");
                return;
            }


            // 3. Bulk Operation (comune e ottimizzato)
            // Usa una transazione per sicurezza
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                // BulkInsertOrUpdate fa l'upsert basandosi sulla Primary Key
                await _dbContext.BulkInsertOrUpdateAsync(entities);
                await transaction.CommitAsync();
            }

            _logger.LogInformation($"Sync completed. Processed {entities.Count} records.");
        }

        protected abstract Task<List<TEntity>> GetEntities(string municipality);
    }
}