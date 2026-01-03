using DataInjection.Collectors;
using DataInjection.Interfaces;

namespace DataInjection;

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
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    await syncService.ExecuteSyncAsync(new ArtCultureCollector(fetcher, configuration));
                }),
                (nameof(PublicEventCollector), async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    await syncService.ExecuteSyncAsync(new PublicEventCollector(fetcher, configuration));
                }),
                (nameof(ArticleCollector), async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                    await syncService.ExecuteSyncAsync(new ArticleCollector(fetcher, configuration));
                }),
                (nameof(NatureCollector), async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                    await syncService.ExecuteSyncAsync(new NatureCollector(fetcher, configuration));
                }),
                (nameof(OrganizationCollector), async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    await syncService.ExecuteSyncAsync(new OrganizationCollector(fetcher, configuration));
                }),
                (nameof(EntertainmentLeisureCardCollector), async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    await syncService.ExecuteSyncAsync(new EntertainmentLeisureCardCollector(fetcher, configuration));
                }),
                (nameof(MunicipalityCardCollector), async () =>
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();
                    var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    await syncService.ExecuteSyncAsync(new MunicipalityCardCollector(fetcher, configuration));
                }),
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

        _logger.Information("Daily data sync finished.");
    }
}
