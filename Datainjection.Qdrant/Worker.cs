using DataInjection.Interfaces;
using DataInjection.Qdrant.Collectors;
using DataInjection.Qdrant.Data;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;

namespace DataInjection.Qdrant
{
    public class Worker(
        IServiceScopeFactory scopeFactory,
        Serilog.ILogger logger) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly Serilog.ILogger _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Daily data sync completed. Starting Qdrant sync...");

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

            var collectors = new List<(string Name, Func<Task> Work)>
            {
                (nameof(ArtCultureQdrantCollector), new Func<Task>(async () =>
                {
                    using var scope = _scopeFactory.CreateScope();

                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var embedder = scope.ServiceProvider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
                    var store = scope.ServiceProvider.GetRequiredService<VectorStoreCollection<Guid, POIEntity>>();
                    var logger = scope.ServiceProvider.GetRequiredService<Serilog.ILogger>();

                    var collector = new ArtCultureQdrantCollector(logger, embedder, configuration, fetcher)!;
                    var syncService = new QdrantEntitySync(logger, configuration, store);
                    await syncService.ExecuteSyncAsync(collector);
                })),
                (nameof(ArticleQdrantCollector), new Func<Task>(async () =>
                {
                    using var scope = _scopeFactory.CreateScope();

                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var embedder = scope.ServiceProvider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
                    var store = scope.ServiceProvider.GetRequiredService<VectorStoreCollection<Guid, POIEntity>>();
                    var logger = scope.ServiceProvider.GetRequiredService<Serilog.ILogger>();

                    var collector = new ArticleQdrantCollector(logger, embedder, configuration, fetcher)!;
                    var syncService = new QdrantEntitySync(logger, configuration, store);
                    await syncService.ExecuteSyncAsync(collector);
                })),
                (nameof(EntertainmentLeisureCardQdrantCollector), new Func<Task>(async () =>
                {
                    using var scope = _scopeFactory.CreateScope();

                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var embedder = scope.ServiceProvider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
                    var store = scope.ServiceProvider.GetRequiredService<VectorStoreCollection<Guid, POIEntity>>();
                    var logger = scope.ServiceProvider.GetRequiredService<Serilog.ILogger>();

                    var collector = new EntertainmentLeisureCardQdrantCollector(logger, embedder, configuration, fetcher)!;
                    var syncService = new QdrantEntitySync(logger, configuration, store);
                    await syncService.ExecuteSyncAsync(collector);
                })),
                (nameof(NatureQdrantCollector), new Func<Task>(async () =>
                {
                    using var scope = _scopeFactory.CreateScope();

                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var embedder = scope.ServiceProvider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
                    var store = scope.ServiceProvider.GetRequiredService<VectorStoreCollection<Guid, POIEntity>>();
                    var logger = scope.ServiceProvider.GetRequiredService<Serilog.ILogger>();

                    var collector = new NatureQdrantCollector(logger, embedder, configuration, fetcher)!;
                    var syncService = new QdrantEntitySync(logger, configuration, store);
                    await syncService.ExecuteSyncAsync(collector);
                })),
                (nameof(OrganizationQdrantCollector), new Func<Task>(async () =>
                {
                    using var scope = _scopeFactory.CreateScope();

                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var embedder = scope.ServiceProvider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
                    var store = scope.ServiceProvider.GetRequiredService<VectorStoreCollection<Guid, POIEntity>>();
                    var logger = scope.ServiceProvider.GetRequiredService<Serilog.ILogger>();

                    var collector = new OrganizationQdrantCollector(logger, embedder, configuration, fetcher)!;
                    var syncService = new QdrantEntitySync(logger, configuration, store);
                    await syncService.ExecuteSyncAsync(collector);
                })),
                (nameof(PublicEventQdrantCollector), new Func<Task>(async () =>
                {
                    using var scope = _scopeFactory.CreateScope();

                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var embedder = scope.ServiceProvider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
                    var store = scope.ServiceProvider.GetRequiredService<VectorStoreCollection<Guid, POIEntity>>();
                    var logger = scope.ServiceProvider.GetRequiredService<Serilog.ILogger>();

                    var collector = new PublicEventQdrantCollector(logger, embedder, configuration, fetcher)!;
                    var syncService = new QdrantEntitySync(logger, configuration, store);
                    await syncService.ExecuteSyncAsync(collector);
                }))
            };


            foreach (var collector in collectors)
            {
                try
                {
                    _logger.Information("Starting qdrant sync for {Collector}", collector.Name);

                    // Execute once at a time
                    await collector.Work();

                    _logger.Information("Successfully synced qdrant data for {Collector}", collector.Name);
                }
                catch (Exception ex)
                {
                    // If one fails, we log and move on to the next without blocking everything
                    _logger.Error(ex, "Failed to sync data for {Collector}", collector.Name);
                }
            }

            _logger.Information("Daily qdrant data sync finished.");

        }

        private async Task ExecuteCollectorAsync<TCollector>() where TCollector : class
        {
            using var scope = _scopeFactory.CreateScope();

            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
            var embedder = scope.ServiceProvider.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
            var store = scope.ServiceProvider.GetRequiredService<VectorStoreCollection<Guid, POIEntity>>();
            var logger = scope.ServiceProvider.GetRequiredService<Serilog.ILogger>();

            var collector = (TCollector)Activator.CreateInstance(typeof(TCollector), logger, embedder, configuration, fetcher)!;
            var syncService = new QdrantEntitySync(logger, configuration, store);
            await syncService.ExecuteSyncAsync((dynamic)collector);
        }
    }
}