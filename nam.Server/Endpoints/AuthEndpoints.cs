using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.DTOs;
using nam.Server.Models.Entities;
using nam.Server.Models.Services.Infrastructure;
using nam.Server.Models.Services.Infrastructure.Repositories;
using System.IdentityModel.Tokens.Jwt;

namespace nam.Server.Endpoints
{
    internal static class AuthEndpoints
    {

        private static Serilog.ILogger? _logger;

        public static void ConfigureLogger(Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Registers a new user.
        /// /// Validates the incoming <paramref name="request"/>, ensures the email is not already in use,
        /// /// hashes the password and persists the new user using <paramref name="userRepository"/>.
        /// </summary>
        /// <param name="request">Registration details.</param>
        /// <param name="userRepository">Repository used to query and persist users.</param>
        /// <param name="validator">FluentValidation validator for <see cref="RegisterUserDto"/>.</param>
        /// <returns>An <see cref="IResult"/> representing the outcome (BadRequest, ValidationProblem, Conflict, Ok, or Problem).</returns>
        public static async Task<IResult> RegisterUser(
            [FromBody] RegisterUserDto request,
            IRegistrationService registrationService,
            IValidator<RegisterUserDto> validator)
        {
            _logger.Information("RegisterUser called for email {Email}", request?.Email);

            ArgumentNullException.ThrowIfNull(registrationService);
            ArgumentNullException.ThrowIfNull(validator);

            try
            {
                if (request == null)
                {
                    _logger.Warning("RegisterUser called with null request");
                    return TypedResults.BadRequest("Request body cannot be null.");
                }

                var validationResult = await validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    _logger.Warning("Validation failed for email {Email}: {@Errors}", request?.Email, validationResult.ToDictionary());
                    return TypedResults.ValidationProblem(validationResult.ToDictionary());
                }

                var created = await registrationService.RegisterUser(request);
                if (!created)
                {
                    _logger.Warning("Attempt to register already existing email {Email}", request.Email);
                    return TypedResults.Conflict("Email is already in use.");
                }

                _logger.Information("User registered successfully with email {Email}", request.Email);
                return TypedResults.Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while registering user with email {Email}", request?.Email);
                return TypedResults.Problem("An error occurred while registering the user.");
            }
        }


        public static async Task<IResult> RequestPasswordReset(
            [FromBody] PasswordResetRequestDto request,
            ApplicationDbContext context,
            IEmailService emailService,
            ICodeService codeService
            )
        {
            // Find a user
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

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

            // Generate Auth code
            var authCode = codeService.GenerateAuthCode();
            var existingCode = await context.ResetPasswordAuth
                    .FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

            if (existingCode != null)
            {
                //delete exist code associated with user
                context.ResetPasswordAuth.Remove(existingCode);
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
            context.ResetPasswordAuth.Add(resetCode);
            await context.SaveChangesAsync();

            // Send email
            await emailService.SendEmailAsync(user.Email, "Reset code", $" AuthCode: {authCode} \n expired: {resetCode.ExpiresAt}");

            var response = new PasswordResetResponseDto
            {
                Success = true,
                Message = "Please check your email; we have sent you the reset code."
            };
            return TypedResults.Ok(response);
        }
        public static async Task<IResult> VerifyAuthCode(
                        [FromBody] ValidationCodeDto request,
                        ApplicationDbContext context)
        {

            // Find and validation reset code
            var resetCode = await context.ResetPasswordAuth
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
        public static async Task<IResult> ResetPassword(
            [FromBody] PasswordResetConfirmDto request,
            ApplicationDbContext context)
        {
            // Find and validation reset code
            var resetCode = await context.ResetPasswordAuth
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
            var user = await context.Users
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


            //TODO: replace with service for encrypt data
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            // Delete resetCode
            context.ResetPasswordAuth.Remove(resetCode);

            await context.SaveChangesAsync();

            return TypedResults.Ok(
                new PasswordResetResponseDto
                {
                    Success = true,
                    Message = "Password successfully reset."
                }
               );
        }

        public static async Task<IResult> GenerateToken(
        [FromServices] IAuthService authService,
        [FromBody] LoginCredentialsDto credentials)
        {
            string? token = await authService.GenerateTokenAsync(credentials);

            if (string.IsNullOrEmpty(token))
            {
                return Results.Unauthorized();
            }

            return Results.Ok(new { token });
        }

        // POST /logout
        public static async Task<IResult> LogoutAsync(
            HttpContext httpContext,
            [FromServices] ITokenService tokenService,
            CancellationToken cancellationToken)
        {
            var user = httpContext.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                return Results.Unauthorized();
            }

            var jti = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            var expString = user.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;

            if (!long.TryParse(expString, out var expSeconds))
            {
                return Results.BadRequest("Claim exp not valid.");
            }

            var expiresAt = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;

            await tokenService.RevokeTokenAsync(jti, expiresAt, cancellationToken);

            return Results.Ok(new { message = "Logout done, token revokated." });

        }
    }
}
