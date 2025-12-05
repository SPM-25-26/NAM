using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nam.Server.Models.ApiResponse;
using nam.Server.Models.DTOs;
using nam.Server.Models.Services.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace nam.Server.Endpoints
{
    internal static class AuthEndpoints
    {
        private static Serilog.ILogger? _logger;

        public static void ConfigureLogger(Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public static async Task<IResult> RegisterUser(
            [FromBody] RegisterUserDto request,
            IAuthService authService,
            IValidator<RegisterUserDto> validator)
        {
            _logger?.Information("RegisterUser called for email {Email}", request?.Email);

            ArgumentNullException.ThrowIfNull(authService);
            ArgumentNullException.ThrowIfNull(validator);

            try
            {
                if (request == null)
                {
                    _logger?.Warning("RegisterUser called with null request");
                    return TypedResults.BadRequest(ApiResponse<object>.Fail("Request body cannot be null."));
                }

                var validationResult = await validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    _logger?.Warning("Validation failed for email {Email}: {@Errors}", request?.Email, validationResult.ToDictionary());
                    // Nota: qui potresti voler strutturare meglio gli errori di validazione nel Data, 
                    // ma per ora manteniamo il messaggio generico o usiamo i dizionari.
                    return TypedResults.ValidationProblem(validationResult.ToDictionary());
                }

                var success = await authService.RegisterUser(request);
                if (!success)
                {
                    _logger?.Warning("Attempt to register already existing email {Email}", request.Email);
                    return TypedResults.Conflict(ApiResponse<object>.Fail("Email is already in use."));
                }

                _logger?.Information("User registered successfully with email {Email}", request.Email);
                return TypedResults.Ok(ApiResponse<object>.Ok(null, "User registered successfully"));
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error while registering user with email {Email}", request?.Email);
                return TypedResults.Problem("An error occurred while registering the user.");
            }
        }

        public static async Task<IResult> RequestPasswordReset(
            [FromBody] PasswordResetRequestDto request, IAuthService authService)
        {
            var result = await authService.RequestPasswordReset(request);

            if (!result.Success)
            {
                // Se l'utente non è stato trovato, per sicurezza spesso si risponde OK comunque, 
                // ma qui rispetto la tua logica originale che restituiva NotFound/BadRequest.
                if (result.Message == "The email not found")
                    return TypedResults.NotFound(ApiResponse<PasswordResetResponseDto>.Fail(result.Message, null, result));

                return TypedResults.BadRequest(ApiResponse<PasswordResetResponseDto>.Fail(result.Message, null, result));
            }

            return TypedResults.Ok(ApiResponse<PasswordResetResponseDto>.Ok(result, result.Message));
        }

        public static async Task<IResult> VerifyAuthCode(
                        [FromBody] ValidationCodeDto request, IAuthService authService)
        {
            var result = await authService.VerifyAuthCode(request);

            if (!result.Success)
            {
                return TypedResults.BadRequest(ApiResponse<PasswordResetResponseDto>.Fail(result.Message, null, result));
            }

            return TypedResults.Ok(ApiResponse<PasswordResetResponseDto>.Ok(result, result.Message));
        }

        public static async Task<IResult> ResetPassword(
            [FromBody] PasswordResetConfirmDto request, IAuthService authService)
        {
            var result = await authService.ResetPassword(request);

            if (!result.Success)
            {
                return TypedResults.BadRequest(ApiResponse<PasswordResetResponseDto>.Fail(result.Message, null, result));
            }

            return TypedResults.Ok(ApiResponse<PasswordResetResponseDto>.Ok(result, result.Message));
        }

        public static async Task<IResult> GenerateToken(
            [FromBody] LoginCredentialsDto credentials, IAuthService authService)
        {
            string? token = await authService.GenerateTokenAsync(credentials);

            if (string.IsNullOrEmpty(token))
            {
                return TypedResults.Unauthorized();
                // O se vuoi usare ApiResponse anche qui:
                // return TypedResults.Json(ApiResponse<object>.Fail("Unauthorized"), statusCode: 401);
            }

            return TypedResults.Ok(ApiResponse<object>.Ok(new { token }));
        }

        public static async Task<IResult> Login(
            HttpContext httpContext,
            [FromBody] LoginCredentialsDto credentials, IAuthService authService)
        {
            string? token = await authService.GenerateTokenAsync(credentials);

            if (string.IsNullOrEmpty(token))
            {
                // return TypedResults.Unauthorized();
                return TypedResults.Json(ApiResponse<object>.Fail("Invalid credentials or email not verified"), statusCode: 401);
            }

            httpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                Path = "/"
            });

            return TypedResults.Ok(ApiResponse<object>.Ok(new
            {
                tokenSetInCookie = true
            }, "Logged in"));
        }

        public static IResult ValidateToken(ClaimsPrincipal user)
        {
            //If the code reaches here, it means the cookie is valid and the user is authenticated.

            return TypedResults.Ok(ApiResponse<object>.Ok(null, "Token is valid."));
        }

        public static async Task<IResult> LogoutAsync(
            HttpContext httpContext,
            CancellationToken cancellationToken, IAuthService authService)
        {
            var user = httpContext.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                return TypedResults.Json(ApiResponse<object>.Fail("User not authenticated"), statusCode: 401);
            }

            var jti = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            var expString = user.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;

            if (!long.TryParse(expString, out var expSeconds))
            {
                return TypedResults.BadRequest(ApiResponse<object>.Fail("Claim exp not valid."));
            }

            var expiresAt = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;

            await authService.RevokeTokenAsync(jti, expiresAt, cancellationToken);

            // Delete the AuthToken cookie
            httpContext.Response.Cookies.Delete("AuthToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return TypedResults.Ok(ApiResponse<object>.Ok(null, "Logout done, token revokated."));
        }

        public static async Task<IResult> VerifyEmail(
            [FromServices] IAuthService authService,
            [FromBody] VerifyEmailRequestDto request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Token))
            {
                return TypedResults.BadRequest(ApiResponse<object>.Fail("Email or token missing."));
            }

            var result = await authService.VerifyEmailAsync(request.Token, cancellationToken);

            if (!result)
            {
                return TypedResults.BadRequest(ApiResponse<object>.Fail("Verification failed."));
            }

            return TypedResults.Ok(ApiResponse<object>.Ok(null, "Email successfully verified."));
        }
    }
}