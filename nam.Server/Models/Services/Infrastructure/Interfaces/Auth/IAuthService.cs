using nam.Server.Models.DTOs;

namespace nam.Server.Models.Services.Infrastructure.Interfaces.Auth
{
    public interface IAuthService
    {

        Task<string?> GenerateTokenAsync(LoginCredentialsDto credentials, CancellationToken cancellationToken = default);

        /// <summary>
        /// Registers a new user using the provided registration data transfer object.
        /// </summary>
        /// <param name="registerUserDto">The DTO containing the information required to register the user.</param>
        /// <param name="cancellationToken">Optional cancellation token to cancel the operation.</param>
        /// <returns>
        /// A task that returns <c>true</c> if the registration succeeded; otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="registerUserDto"/> is <c>null</c>.</exception>
        Task<bool> RegisterUser(RegisterUserDto registerUserDto, CancellationToken cancellationToken = default);

        Task RevokeTokenAsync(string jti, DateTime expiresAt, CancellationToken cancellationToken = default);
        Task<bool> IsTokenRevokedAsync(string jti, CancellationToken cancellationToken = default);
        Task<int> CleanupExpiredRevokedTokensAsync(CancellationToken cancellationToken = default);

        Task<PasswordResetResponseDto> RequestPasswordReset(PasswordResetRequestDto request);
        Task<PasswordResetResponseDto> ResetPassword(PasswordResetConfirmDto request);
        Task<PasswordResetResponseDto> VerifyAuthCode(ValidationCodeDto request);
        Task<bool> ValidateToken(string userEmail);
        Task<bool> VerifyEmailAsync(string token, CancellationToken cancellationToken);
    }
}
