using nam.Server.Models.DTOs;

namespace nam.Server.Models.Services.Infrastructure
{
    public interface IRegistrationService
    {
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
    }
}
