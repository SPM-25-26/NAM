using nam.Server.Models.DTOs;
using System.Security.Claims;

namespace nam.Server.Models.Services.Infrastructure
{
    public interface ITokenGeneration
    {
        Task<string?> GenerateTokenAsync(string userId, string email);
        ClaimsPrincipal? ValidateEmailVerificationToken(string token);
    }
}
