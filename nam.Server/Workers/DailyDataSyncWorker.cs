using nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection;
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

            var artCultureCollector = new ArtCultureCollector(fetcher);

            //using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            //{
            //    // Deletes the DB entirely and recreates it (Clears Data AND Schema)
            //    context.Database.EnsureDeleted();
            //    context.Database.EnsureCreated();
            //    _logger.Warning("Database reset completed.");
            //}

            try
            {
                await syncService.ExecuteSyncAsync(artCultureCollector);

                _logger.Information("Successfully synced data");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to sync data.");
            }
        }
    }
}
