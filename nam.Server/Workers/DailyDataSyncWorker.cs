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

            using var scope = _scopeFactory.CreateScope();

            var fetcher = scope.ServiceProvider.GetRequiredService<IFetcher>();

            var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();

            // List of collectors: add new collectors here.
            var collectors = new List<(string Name, Func<Task> Work)>
            {
                ("ArtCultureCollector", () => syncService.ExecuteSyncAsync(new ArtCultureCollector(fetcher)))
                // Add other collectors using the same pattern:
                // ("OtherCollector", () => syncService.ExecuteSyncAsync(new OtherCollector(fetcher)))
            };

            // Execute collectors in parallel, each with isolated logging and error handling.
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
