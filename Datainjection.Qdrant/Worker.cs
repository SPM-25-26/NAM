using Datainjection.Qdrant.Sync;
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

            using var scope = _scopeFactory.CreateScope();
            var sync = scope.ServiceProvider.GetRequiredService<QdrantEntitySync>();

            await sync.ExecuteSyncAsync();

            _logger.Information("Daily qdrant data sync finished.");

        }
    }
}