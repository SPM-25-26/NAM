using nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection;

namespace nam.Server.Workers
{
    public class DailyDataSyncWorker(IServiceScopeFactory scopeFactory, ILogger<DailyDataSyncWorker> logger) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<DailyDataSyncWorker> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var timer = new PeriodicTimer(TimeSpan.FromHours(24));

                // Run immediately on startup before waiting 24h
                await DoWorkAsync(stoppingToken);

                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await DoWorkAsync(stoppingToken);
                }
            }
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting daily data sync...");

            using var scope = _scopeFactory.CreateScope();
            var apiService = scope.ServiceProvider.GetRequiredService<ArtCultureSyncService>();

            try
            {
                await apiService.ExecuteSyncAsync();

                _logger.LogInformation($"Successfully synced data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to sync data.");
            }
        }
    }
}