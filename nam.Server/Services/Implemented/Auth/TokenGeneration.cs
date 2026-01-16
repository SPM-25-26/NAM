using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using nam.Server.Options;
using nam.Server.Services.Interfaces.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace nam.Server.Services.Implemented.Auth
{
    public class TokenGeneration(IOptionsMonitor<JwtOptions> jwtOptions) : ITokenGeneration
    {
        public async Task<string?> GenerateTokenAsync(string userId, string email)
        {
            // 1. Claims
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            // 2. Security Key
            var key = jwtOptions.CurrentValue.Secret;
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(keyBytes);

            // 3. Token creation
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal? ValidateEmailVerificationToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtOptions.CurrentValue.Secret);

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2)
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParams, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
