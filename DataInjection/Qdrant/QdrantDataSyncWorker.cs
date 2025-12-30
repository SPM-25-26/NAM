using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Mappers;
using Domain.Entities.MunicipalityEntities;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;

namespace DataInjection.Qdrant
{
    public class QdrantDataSyncWorker(
        IServiceScopeFactory scopeFactory,
        Serilog.ILogger logger) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly Serilog.ILogger _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromHours(24));

            await DoWorkAsync();

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWorkAsync();
            }
        }
        private async Task DoWorkAsync()
        {
            _logger.Information("Starting daily qdrant data sync...");
            var collectionName = "test_sync";

            var collectors = new List<(string Name, Func<Task> Work)>
            {
                (nameof(POIVectorEntityCollector<ArtCultureNatureCard>), new Func<Task>(async () =>
                {
                    using var scope = _scopeFactory.CreateScope();

                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var embedder = scope.ServiceProvider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
                    var store = scope.ServiceProvider.GetRequiredService<VectorStoreCollection<Guid, POIEntity>>();
                    var logger = scope.ServiceProvider.GetRequiredService<Serilog.ILogger>();

                    var collector = new ArtCultureQdrantCollector(embedder, configuration, fetcher);
                    var syncService = new QdrantEntitySync(logger, configuration, store);
                    await syncService.ExecuteSyncAsync(collector);
                }))
            };


            foreach (var collector in collectors)
            {
                try
                {
                    _logger.Information("Starting sync for {Collector}", collector.Name);

                    // Execute once at a time
                    await collector.Work();

                    _logger.Information("Successfully synced data for {Collector}", collector.Name);
                }
                catch (Exception ex)
                {
                    // If one fails, we log and move on to the next without blocking everything
                    _logger.Error(ex, "Failed to sync data for {Collector}", collector.Name);
                }
            }

            _logger.Information("Daily qdrant data sync finished.");

        }
    }
}