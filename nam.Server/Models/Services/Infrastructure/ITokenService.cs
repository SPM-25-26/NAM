namespace nam.Server.Models.Services.Infrastructure
{
    public interface ITokenService
    {
        Task RevokeTokenAsync(string jti, DateTime expiresAt, CancellationToken cancellationToken = default);
        Task<bool> IsTokenRevokedAsync(string jti, CancellationToken cancellationToken = default);
        Task<int> CleanupExpiredRevokedTokensAsync(CancellationToken cancellationToken = default);
    }
}
