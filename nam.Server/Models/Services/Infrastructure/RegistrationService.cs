using nam.Server.Models.DTOs;
using nam.Server.Models.Entities;

namespace nam.Server.Models.Services.Infrastructure
{
    public class RegistrationService(UnitOfWork unitOfWork) : IRegistrationService
    {
        public async Task<bool> RegisterUser(RegisterUserDto registerUserDto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(registerUserDto);

            // verify if a user with the same email already exists
            var exists = await unitOfWork.Users.EmailExistsAsync(registerUserDto.Email, cancellationToken);
            if (exists)
                return false;

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password);
            var newUser = new User
            {
                Email = registerUserDto.Email.Trim(),
                PasswordHash = passwordHash
            };

            var added = await unitOfWork.Users.AddAsync(newUser, cancellationToken);
            return added;
        }
    }
}
