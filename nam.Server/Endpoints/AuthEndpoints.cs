using FluentValidation;
using Microsoft.AspNetCore.Http;
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
            IAuthService _authService,
            IValidator<RegisterUserDto> validator)
        {
            _logger.Information("RegisterUser called for email {Email}", request?.Email);

            ArgumentNullException.ThrowIfNull(_authService);
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

                var created = await _authService.RegisterUser(request);
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
            [FromBody] PasswordResetRequestDto request, IAuthService _authService)
        {
            return await _authService.RequestPasswordReset(request);

        }
        public static async Task<IResult> VerifyAuthCode(
                        [FromBody] ValidationCodeDto request, IAuthService _authService)
        {
            return await _authService.VerifyAuthCode(request);
        }
        public static async Task<IResult> ResetPassword(
            [FromBody] PasswordResetConfirmDto request, IAuthService _authService)
        {
            return await _authService.ResetPassword(request);
        }

        public static async Task<IResult> GenerateToken(
            [FromBody] LoginCredentialsDto credentials, IAuthService _authService)
        {
            string? token = await _authService.GenerateTokenAsync(credentials);

            if (string.IsNullOrEmpty(token))
            {
                return Results.Unauthorized();
            }

            return Results.Ok(new { token });
        }

        public static async Task<IResult> Login(
        HttpContext httpContext,
        [FromBody] LoginCredentialsDto credentials, IAuthService _authService)
        {
            string? token = await _authService.GenerateTokenAsync(credentials);

            if (string.IsNullOrEmpty(token))
            {
                return Results.Unauthorized();
            }

            httpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                Path = "/"
            });

            return Results.Ok(new
            {
                message = "Logged in",
                tokenSetInCookie = true
            });
        }

        // POST /logout
        public static async Task<IResult> LogoutAsync(
            HttpContext httpContext,
            CancellationToken cancellationToken, IAuthService _authService)
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

            await _authService.RevokeTokenAsync(jti, expiresAt, cancellationToken);

            // Delete the AuthToken cookie, even if it's not present
            httpContext.Response.Cookies.Delete("AuthToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Results.Ok(new { message = "Logout done, token revoked." });

        }

        public static async Task<IResult> VerifyEmail(
        [FromServices] IAuthService authService,
        [FromBody] VerifyEmailRequestDto request,
        CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Token))
            {
                return Results.BadRequest("Email or token missing.");
            }

            var result = await authService.VerifyEmailAsync(
                request.Token,
                cancellationToken);

            if (!result)
            {
                return Results.BadRequest( "Verification failed.");
            }

            return Results.Ok(new { message = "Email successfully verified." });
        }
    }
}
