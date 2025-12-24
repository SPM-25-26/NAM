using Domain.Entities;
using Domain.Entities.Auth;
using Infrastructure;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using nam.Server.Models.DTOs;
using nam.Server.Models.Services.Infrastructure.Interfaces.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace nam.Server.Models.Services.Infrastructure.Implemented.Auth
{
    public class AuthService(
        IUnitOfWork unitOfWork,
        ITokenGeneration tokenGeneration,
        IConfiguration configuration,
        ApplicationDbContext _context,
        IEmailService emailService,
        ICodeService codeService) : IAuthService
    {

        public async Task<string?> GenerateTokenAsync(LoginCredentialsDto credentials, CancellationToken cancellationToken = default)
        {
            // 1. Retrive from DB
            var user = await unitOfWork.Users.GetByEmailAsync(credentials.Email, cancellationToken);

            if (user is null) return null;

            if (!user.IsEmailVerified) return null;

            // 2. Password verification with BCrypt
            var passwordValid = BCrypt.Net.BCrypt.Verify(credentials.Password, user.PasswordHash);

            if (!passwordValid) return null;

            return await tokenGeneration.GenerateTokenAsync(user.Id.ToString(), user.Email);
        }

        public async Task<bool> RegisterUser(RegisterUserDto registerUserDto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(registerUserDto);

            var exists = await unitOfWork.Users.EmailExistsAsync(registerUserDto.Email, cancellationToken);
            if (exists) return false;

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password);
            var newUser = new User
            {
                Email = registerUserDto.Email.Trim(),
                PasswordHash = passwordHash
            };

            var added = await unitOfWork.Users.AddAsync(newUser, cancellationToken);
            if (!added) return false;

            var verificationToken = await tokenGeneration.GenerateTokenAsync(newUser.Id.ToString(), newUser.Email);
            var frontendBaseUrl = configuration["Frontend:BaseUrl"] ?? "http://localhost:5173";

            var verificationLink = $"{frontendBaseUrl}/verify-email" +
                $"?email={Uri.EscapeDataString(registerUserDto.Email)}" +
                $"&token={Uri.EscapeDataString(verificationToken)}";

            await emailService.SendEmailAsync(
                registerUserDto.Email,
                "Verify your email",
                $"""
                Ciao,
                Clicca sul link seguente per verificare la tua email:
                {verificationLink}
                Se non hai creato tu questo account, ignora questa email.
                """);
            return added;
        }

        public async Task RevokeTokenAsync(string jti, DateTime expiresAt, CancellationToken cancellationToken = default)
        {
            var alreadyRevoked = await _context.RevokedTokens.AnyAsync(t => t.Jti == jti, cancellationToken);

            if (alreadyRevoked) return;

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
            return _context.RevokedTokens.AnyAsync(t => t.Jti == jti, cancellationToken);
        }

        public async Task<int> CleanupExpiredRevokedTokensAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var expired = await _context.RevokedTokens
                .Where(t => t.ExpiresAt <= now)
                .ToListAsync(cancellationToken);

            if (expired.Count == 0) return 0;

            _context.RevokedTokens.RemoveRange(expired);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        // Returns Dto
        public async Task<PasswordResetResponseDto> RequestPasswordReset(PasswordResetRequestDto request)
        {
            var user = await unitOfWork.Users.GetByEmailAsync(request.Email);

            if (user == null)
            {
                return new PasswordResetResponseDto
                {
                    Success = false,
                    Message = "The email not found"
                };
            }

            if (!user.IsEmailVerified)
            {
                return new PasswordResetResponseDto
                {
                    Success = false,
                    Message = "Email not verified. Please verify your email before requesting a password reset."
                };
            }

            var authCode = codeService.GenerateAuthCode();
            var existingCode = await _context.ResetPasswordAuth
                    .FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

            if (existingCode != null)
            {
                _context.ResetPasswordAuth.Remove(existingCode);
            }

            var resetCode = new PasswordResetCode
            {
                UserId = user.Id.ToString(),
                AuthCode = authCode,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(codeService.TimeToLiveMinutes)
            };

            _context.ResetPasswordAuth.Add(resetCode);
            await _context.SaveChangesAsync();

            await emailService.SendEmailAsync(user.Email, "Reset code", $" AuthCode: {authCode} \n expired: {resetCode.ExpiresAt}");

            return new PasswordResetResponseDto
            {
                Success = true,
                Message = "Please check your email; we have sent you the reset code."
            };
        }

        // Returns DTO
        public async Task<PasswordResetResponseDto> VerifyAuthCode(ValidationCodeDto request)
        {
            var resetCode = await _context.ResetPasswordAuth
                .FirstOrDefaultAsync(c =>
                    c.AuthCode == request.AuthCode &&
                    c.ExpiresAt > DateTime.UtcNow);

            if (resetCode == null)
            {
                return new PasswordResetResponseDto
                {
                    Success = false,
                    Message = "Invalid or expired auth code."
                };
            }

            return new PasswordResetResponseDto
            {
                Success = true,
                Message = "The code is valid"
            };
        }

        // Returns DTO
        public async Task<PasswordResetResponseDto> ResetPassword(PasswordResetConfirmDto request)
        {
            var resetCode = await _context.ResetPasswordAuth
                .FirstOrDefaultAsync(c =>
                    c.AuthCode == request.AuthCode &&
                    c.ExpiresAt > DateTime.UtcNow);

            if (resetCode == null)
            {
                return new PasswordResetResponseDto
                {
                    Success = false,
                    Message = "Invalid or expired auth code."
                };
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id.ToString() == resetCode.UserId);

            if (user == null)
            {
                return new PasswordResetResponseDto
                {
                    Success = false,
                    Message = "Invalid auth code."
                };
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            _context.ResetPasswordAuth.Remove(resetCode);
            await _context.SaveChangesAsync();

            return new PasswordResetResponseDto
            {
                Success = true,
                Message = "Password successfully reset."
            };
        }

        public async Task<bool> VerifyEmailAsync(string token, CancellationToken cancellationToken)
        {
            var principal = tokenGeneration.ValidateEmailVerificationToken(token);
            if (principal is null) return false;

            var email = principal.FindFirst(ClaimTypes.Email)?.Value
                     ?? principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

            if (string.IsNullOrEmpty(email)) return false;

            var user = await unitOfWork.Users.GetByEmailAsync(email, cancellationToken);
            if (user is null) return false;

            if (user.IsEmailVerified) return true;

            user.IsEmailVerified = true;
            await unitOfWork.Users.UpdateAsync(user, cancellationToken);

            return true;
        }

        public Task<bool> ValidateToken(string userEmail)
        {
            return unitOfWork.Users.EmailExistsAsync(userEmail);

        }
    }
}