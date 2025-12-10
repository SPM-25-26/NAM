using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.DTOs;
using nam.Server.Models.Entities;
using nam.Server.Models.Entities.Auth;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.Auth
{
    public class AuthService(IUnitOfWork unitOfWork, ITokenGeneration tokenGeneration, IConfiguration configuration, ApplicationDbContext _context, IEmailService emailService, ICodeService codeService) : IAuthService
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

            if (!user.IsEmailVerified)
            {
                // email not verified
                return null;
            }

            // 2. Password verification with BCrypt
            var passwordValid = BCrypt.Net.BCrypt.Verify(credentials.Password, user.PasswordHash);

            if (!passwordValid)
            {
                // password wrong
                return null;
            }

            return await tokenGeneration.GenerateTokenAsync(user.Id.ToString(), user.Email);

        }

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
            if (!added)
                return false;

            // Construct the link
            var verificationToken = await tokenGeneration.GenerateTokenAsync(newUser.Id.ToString(), newUser.Email);
            var frontendBaseUrl = configuration["Frontend:BaseUrl"]
                ?? "http://localhost:5173"; // default in dev

            var verificationLink = $"{frontendBaseUrl}/verify-email" +
                $"?email={Uri.EscapeDataString(registerUserDto.Email)}" +
                $"&token={Uri.EscapeDataString(verificationToken)}";

            // 3. Invia la mail
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
            // Evita duplicati
            var alreadyRevoked = await _context.RevokedTokens
                .AnyAsync(t => t.Jti == jti, cancellationToken);

            if (alreadyRevoked)
                return;

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
            return _context.RevokedTokens
                .AnyAsync(t => t.Jti == jti, cancellationToken);
        }

        public async Task<int> CleanupExpiredRevokedTokensAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            var expired = await _context.RevokedTokens
                .Where(t => t.ExpiresAt <= now)
                .ToListAsync(cancellationToken);

            if (expired.Count == 0)
                return 0;

            _context.RevokedTokens.RemoveRange(expired);
            return await _context.SaveChangesAsync(cancellationToken);
        }


        public async Task<IResult> RequestPasswordReset(PasswordResetRequestDto request)
        {
            // Find a user
            var user = await unitOfWork.Users.GetByEmailAsync(request.Email);

            // Check find a user
            if (user == null)
            {
                var notFoundResponse = new PasswordResetResponseDto
                {
                    Success = false,
                    Message = "The email not found"
                };
                return TypedResults.NotFound(notFoundResponse);
            }

            if (!user.IsEmailVerified)
            {
                var notVerifiedResponse = new PasswordResetResponseDto
                {
                    Success = false,
                    Message = "Email not verified. Please verify your email before requesting a password reset."
                };
                return TypedResults.BadRequest(notVerifiedResponse);
            }

            // Generate Auth code
            var authCode = codeService.GenerateAuthCode();
            var existingCode = await _context.ResetPasswordAuth
                    .FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

            if (existingCode != null)
            {
                //delete exist code associated with user
                _context.ResetPasswordAuth.Remove(existingCode);
            }

            // Build a response
            var resetCode = new PasswordResetCode
            {
                UserId = user.Id.ToString(),
                AuthCode = authCode,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(codeService.TimeToLiveMinutes)
            };

            // Save reset code in dedicated table
            _context.ResetPasswordAuth.Add(resetCode);
            await _context.SaveChangesAsync();

            // Send email, using the localEmailService for demo purposes (it works also with EmailService)
            await emailService.SendEmailAsync(user.Email, "Reset code", $" AuthCode: {authCode} \n expired: {resetCode.ExpiresAt}");

            var response = new PasswordResetResponseDto
            {
                Success = true,
                Message = "Please check your email; we have sent you the reset code."
            };
            return TypedResults.Ok(response);
        }
        public async Task<IResult> VerifyAuthCode(ValidationCodeDto request)
        {

            // Find and validation reset code
            var resetCode = await _context.ResetPasswordAuth
                .FirstOrDefaultAsync(c =>
                    c.AuthCode == request.AuthCode &&
                    c.ExpiresAt > DateTime.UtcNow);

            // Verify the validity of the reset code
            if (resetCode == null)
            {
                return TypedResults.BadRequest(new PasswordResetResponseDto
                {
                    Success = false,
                    Message = "Invalid or expired auth code."
                });
            }
            else
                return TypedResults.Ok(
                    new PasswordResetResponseDto
                    {
                        Success = true,
                        Message = "The code is valid"
                    }
                );
        }
        public async Task<IResult> ResetPassword(PasswordResetConfirmDto request)
        {
            // Find and validation reset code
            var resetCode = await _context.ResetPasswordAuth
                .FirstOrDefaultAsync(c =>
                    c.AuthCode == request.AuthCode &&
                    c.ExpiresAt > DateTime.UtcNow);

            // Verify the validity of the reset code
            if (resetCode == null)
            {
                var notFoundResponse = new PasswordResetResponseDto
                {
                    Success = false,
                    Message = "Invalid or expired auth code."
                };
                return TypedResults.BadRequest(notFoundResponse);
            }

            // Find the user
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id.ToString() == resetCode.UserId);

            if (user == null)
            {
                var notFoundUserResponse = new PasswordResetResponseDto
                {
                    Success = false,
                    Message = "Invalid auth code."
                };
                return TypedResults.BadRequest(notFoundUserResponse);
            }


            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            // Delete resetCode
            _context.ResetPasswordAuth.Remove(resetCode);

            await _context.SaveChangesAsync();

            return TypedResults.Ok(
                new PasswordResetResponseDto
                {
                    Success = true,
                    Message = "Password successfully reset."
                }
               );
        }



        public async Task<bool> VerifyEmailAsync(string token, CancellationToken cancellationToken)
        {
            // 1. Valida il JWT
            var principal = tokenGeneration.ValidateEmailVerificationToken(token);
            if (principal is null)
            {
                // Firma sbagliata / exp scaduto
                return false;
            }

            // 2. Prendi l'email dal claim
            var email = principal.FindFirst(ClaimTypes.Email)?.Value
                     ?? principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            // 3. Carica l'utente dal repository
            var user = await unitOfWork.Users.GetByEmailAsync(email, cancellationToken);
            if (user is null)
            {
                return false;
            }

            // 4. Se già verificato -> true
            if (user.IsEmailVerified)
            {
                return true;
            }

            // 5. Marca come verificato
            user.IsEmailVerified = true;
            await unitOfWork.Users.UpdateAsync(user, cancellationToken);

            return true;
        }

    }
}
