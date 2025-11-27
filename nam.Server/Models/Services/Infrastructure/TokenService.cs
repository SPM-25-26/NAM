using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.Entities;

namespace nam.Server.Models.Services.Infrastructure
{
    public class TokenService : ITokenService
    {
        private readonly ApplicationDbContext _context;

        public TokenService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RevokeTokenAsync(string jti, DateTime expiresAt, CancellationToken cancellationToken = default)
        {
            // Evita duplicati
            var alreadyRevoked = await _context.RevokedTokens
                .AnyAsync(t => t.Jti == jti, cancellationToken);

            if (alreadyRevoked)
                return;

            var entity = new RevokedToken
            {
                Jti = jti,
                ExpiresAt = expiresAt
            };

            _context.RevokedTokens.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task<bool> IsTokenRevokedAsync(string jti, CancellationToken cancellationToken = default)
        {
            return _context.RevokedTokens
                .AnyAsync(t => t.Jti == jti, cancellationToken);
        }

        public async Task<int> CleanupExpiredRevokedTokensAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            var expired = await _context.RevokedTokens
                .Where(t => t.ExpiresAt <= now)
                .ToListAsync(cancellationToken);

            if (expired.Count == 0)
                return 0;

            _context.RevokedTokens.RemoveRange(expired);
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
