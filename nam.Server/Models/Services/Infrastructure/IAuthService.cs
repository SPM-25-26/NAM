using nam.Server.Models.DTOs;

namespace nam.Server.Models.Services.Infrastructure
{
    public interface IAuthService
    {

        Task<string?> GenerateTokenAsync(LoginCredentialsDto credentials, CancellationToken cancellationToken = default);
    }
}
