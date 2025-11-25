using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using nam.Server.Models.DTOs;
using nam.Server.Models.Options;
using nam.Server.Models.Services.Infrastructure.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace nam.Server.Models.Services.Infrastructure
{
    public class AuthService(IUnitOfWork unitOfWork, IOptionsMonitor<JwtOptions> jwtOptions) : IAuthService
    {

        public async Task<string?> GenerateTokenAsync(LoginCredentialsDto credentials, CancellationToken cancellationToken = default)
        {
            // 1. Retrive from DB
            var user = await unitOfWork.Users.GetByEmailAsync(credentials.Email, cancellationToken);

            if (user is null)
            {
                // user not found
                return null;
            }

            // 2. Password verification with BCrypt
            var passwordValid = BCrypt.Net.BCrypt.Verify(credentials.Password, user.PasswordHash);

            if (!passwordValid)
            {
                // password wrong
                return null;
            }

            // 3. Claims
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            // 4. Security Key
            var key = jwtOptions.CurrentValue.Secret;
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(keyBytes);

            // 5. Token creation
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
                // opzionale: Issuer, Audience se li usi
                // Issuer = _jwtOptions.Value.Issuer,
                // Audience = _jwtOptions.Value.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
