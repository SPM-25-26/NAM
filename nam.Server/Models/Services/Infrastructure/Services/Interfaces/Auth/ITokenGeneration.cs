using System.Security.Claims;

namespace nam.Server.Models.Services.Infrastructure.Services.Interfaces.Auth
{
    public interface ITokenGeneration
    {
        Task<string?> GenerateTokenAsync(string userId, string email);
        ClaimsPrincipal? ValidateEmailVerificationToken(string token);
    }
}
