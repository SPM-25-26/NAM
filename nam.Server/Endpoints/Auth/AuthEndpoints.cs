using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using nam.Server.Models.DTOs;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.Auth;
using System.IdentityModel.Tokens.Jwt;

namespace nam.Server.Endpoints.Auth
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
            _logger?.Information("RegisterUser called for email {Email}", request?.Email);

            ArgumentNullException.ThrowIfNull(_authService);
            ArgumentNullException.ThrowIfNull(validator);

            try
            {
                if (request == null)
                {
                    _logger?.Warning("RegisterUser called with null request");
                    return TypedResults.BadRequest("Request body cannot be null.");
                }

                var validationResult = await validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    _logger?.Warning("Validation failed for email {Email}: {@Errors}", request?.Email, validationResult.ToDictionary());
                    return TypedResults.ValidationProblem(validationResult.ToDictionary());
                }

                var created = await _authService.RegisterUser(request);
                if (!created)
                {
                    _logger?.Warning("Attempt to register already existing email {Email}", request.Email);
                    return TypedResults.Conflict("Email is already in use.");
                }

                _logger?.Information("User registered successfully with email {Email}", request.Email);
                return TypedResults.Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error while registering user with email {Email}", request?.Email);
                return TypedResults.Problem("An error occurred while registering the user.");
            }
        }

        /// <summary>
        /// Handles a request for initiating a password reset process.
        /// </summary>
        /// <remarks>
        /// This endpoint finds the user by email, generates a unique authentication code, 
        /// persists the code with an expiry time, and sends the code to the user's email 
        /// address for verification. If an existing code for the user is found, it is 
        /// replaced with the new one.
        /// </remarks>
        /// <param name="request">The DTO containing the user's email address.</param>
        /// <param name="context">The application database context for user and code storage.</param>
        /// <param name="emailService">The service for sending the reset code via email.</param>
        /// <param name="codeService">The service for generating the authentication code and managing its time-to-live.</param>
        /// <returns>
        /// A <see cref="TypedResults.Ok"/> with a success message if the email is found and the code is sent. 
        /// A <see cref="TypedResults.NotFound"/> with a failure message if the email is not found.
        /// The response body contains a <see cref="PasswordResetResponseDto"/>.
        /// </returns>
        public static async Task<IResult> RequestPasswordReset(
            [FromBody] PasswordResetRequestDto request, IAuthService _authService)
        {
            return await _authService.RequestPasswordReset(request);

        }

        /// <summary>
        /// Verifies the validity and expiration of a provided authentication code (Auth Code) for password reset.
        /// </summary>
        /// <remarks>
        /// The endpoint searches for a record in <c>ResetPasswordAuth</c> that matches the provided code 
        /// and has an expiration date in the future (<c>ExpiresAt > DateTime.UtcNow</c>). 
        /// If the code is valid, the user is allowed to proceed with the password update.
        /// </remarks>
        /// <param name="request">The DTO containing the authentication code to be validated.</param>
        /// <param name="context">The database context for accessing the reset code table.</param>
        /// <returns>
        /// A <see cref="TypedResults.Ok"/> if the code is valid and not expired. 
        /// A <see cref="TypedResults.BadRequest"/> if the code is not found or has expired.
        /// The response body contains a <see cref="PasswordResetResponseDto"/>.
        /// </returns>
        public static async Task<IResult> VerifyAuthCode(
                        [FromBody] ValidationCodeDto request, IAuthService _authService)
        {
            return await _authService.VerifyAuthCode(request);
        }

        /// <summary>
        /// Finalizes the password reset process by updating the user's password.
        /// </summary>
        /// <remarks>
        /// This endpoint verifies the provided authentication code and confirms its validity and expiration. 
        /// If the code is valid, the user's password is changed (hashed), the reset code is deleted, 
        /// and the changes are persisted to the database.
        /// </remarks>
        /// <param name="request">The DTO containing the authentication code and the new password.</param>
        /// <param name="context">The application database context for accessing user and reset code data.</param>
        /// <returns>
        /// A <see cref="TypedResults.Ok"/> with a success message if the password is reset successfully.
        /// A <see cref="TypedResults.BadRequest"/> with a failure message if the code is invalid, expired, or the user is not found.
        /// The response body contains a <see cref="PasswordResetResponseDto"/>.
        /// </returns>
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

            return Results.Ok(new { message = "Logout done, token revokated." });

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
                return Results.BadRequest("Verification failed.");
            }

            return Results.Ok(new { message = "Email successfully verified." });
        }
    }
}
