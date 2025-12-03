using EFCore.BulkExtensions;
using nam.Server.Data;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;

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
        var dtos = await FetchDataFromApiAsync();

        if (dtos == null || dtos.Count == 0)
        {
            _logger.LogWarning("No data fetched");
            return;
        }

        // 2. Mapping (comune, ma sovrascrivibile se necessario)
        var entities = MapToEntities(dtos);

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

    protected abstract Task<List<TDto>> FetchDataFromApiAsync();
    protected abstract List<TEntity> MapToEntities(List<TDto> dtos);
}
