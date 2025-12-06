using nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Sync;
using Serilog;

namespace nam.Server.Workers
{
    public class DailyDataSyncWorker(
        IServiceScopeFactory scopeFactory,
        Serilog.ILogger logger) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly Serilog.ILogger _logger = Log.Logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromHours(24));

            await DoWorkAsync(stoppingToken);

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWorkAsync(stoppingToken);
            }
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Starting daily data sync...");

            using var scope = _scopeFactory.CreateScope();

            var apiService = scope.ServiceProvider.GetRequiredService<ArtCultureSyncService>();

            try
            {
                await apiService.ExecuteSyncAsync();

                _logger.Information("Successfully synced data");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to sync data.");
            }
        }
    }
}
