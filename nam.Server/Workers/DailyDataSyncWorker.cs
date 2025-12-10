using nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Collectors;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;

namespace nam.Server.Workers
{
    public class DailyDataSyncWorker(
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
            _logger.Information("Starting daily data sync...");

            // List of collectors: add new collectors here.
            var collectors = new List<(string Name, Func<Task> Work)>
            {
                (nameof(ArtCultureCollector), async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                    await syncService.ExecuteSyncAsync(new ArtCultureCollector(fetcher));
                }),
                (nameof(PublicEventCollector), async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                    await syncService.ExecuteSyncAsync(new PublicEventCollector(fetcher));
                }),
                (nameof(ArticleCollector), async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                    await syncService.ExecuteSyncAsync(new ArticleCollector(fetcher));
                }),
                (nameof(NatureCollector), async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                    await syncService.ExecuteSyncAsync(new NatureCollector(fetcher));
                }),
                (nameof(OrganizationCollector), async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                    await syncService.ExecuteSyncAsync(new OrganizationCollector(fetcher));
                }),
                (nameof(EntertainmentLeisureCardCollector), async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                    await syncService.ExecuteSyncAsync(new EntertainmentLeisureCardCollector(fetcher));
                }),
                (nameof(MunicipalityCardCollector), async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                    await syncService.ExecuteSyncAsync(new MunicipalityCardCollector(fetcher));
                }),
            };

            // Execute collectors in parallel, each with isolated scope (and therefore isolated DbContext/transaction).
            var tasks = collectors.Select(async collector =>
            {
                try
                {
                    _logger.Information("Starting sync for {Collector}", collector.Name);
                    await collector.Work();
                    _logger.Information("Successfully synced data for {Collector}", collector.Name);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to sync data for {Collector}", collector.Name);
                }
            }).ToArray();

            await Task.WhenAll(tasks);

            _logger.Information("Daily data sync finished.");
        }
    }
}
