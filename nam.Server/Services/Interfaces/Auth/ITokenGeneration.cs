using System.Security.Claims;

namespace nam.Server.Services.Interfaces.Auth
{
    public interface ITokenGeneration
    {
        Task<string?> GenerateTokenAsync(string userId, string email);
        ClaimsPrincipal? ValidateEmailVerificationToken(string token);
    }
}
