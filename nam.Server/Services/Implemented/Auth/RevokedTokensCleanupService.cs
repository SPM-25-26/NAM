using nam.Server.Services.Interfaces.Auth;

namespace nam.Server.Services.Implemented.Auth
{
    public class RevokedTokensCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RevokedTokensCleanupService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromHours(1);

        public RevokedTokensCleanupService(
            IServiceProvider serviceProvider,
            ILogger<RevokedTokensCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RevokedTokensCleanupService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var tokenService = scope.ServiceProvider.GetRequiredService<IAuthService>();

                    var removed = await tokenService.CleanupExpiredRevokedTokensAsync(stoppingToken);
                    if (removed > 0)
                    {
                        _logger.LogInformation("Cleanup RevokedTokens: removed {Count} expired records.", removed);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during RevokedTokens cleanup.");
                }

                try
                {
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }

            _logger.LogInformation("RevokedTokensCleanupService stopped.");
        }
    }
}