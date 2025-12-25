using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using nam.Server.ApiResponse;
using nam.Server.DTOs;
using nam.Server.Services.Interfaces.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace nam.Server.Endpoints.Auth
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
                    return TypedResults.Problem(detail: "Request body cannot be null.", statusCode: 400);
                }

                var validationResult = await validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    _logger?.Warning("Validation failed for email {Email}: {@Errors}", request?.Email, validationResult.ToDictionary());
                    return TypedResults.ValidationProblem(validationResult.ToDictionary());
                }

                var success = await authService.RegisterUser(request);
                if (!success)
                {
                    _logger?.Warning("Attempt to register already existing email {Email}", request.Email);
                    return TypedResults.Conflict(new MessageResponse("Email is already in use."));
                }

                _logger?.Information("User registered successfully with email {Email}", request.Email);
                return TypedResults.Ok(new MessageResponse("User registered successfully"));
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error while registering user with email {Email}", request?.Email);
                return TypedResults.Problem(detail: "An error occurred while registering the user.", statusCode: 500);
            }
        }

        public static async Task<IResult> RequestPasswordReset(
            [FromBody] PasswordResetRequestDto request, IAuthService authService)
        {
            var result = await authService.RequestPasswordReset(request);

            if (!result.Success)
            {
                if (result.Message == "The email not found")
                    return TypedResults.Problem(detail: result.Message, statusCode: 404);

                return TypedResults.Problem(detail: result.Message, statusCode: 400);
            }

            return TypedResults.Ok(new { message = result.Message, data = result });
        }

        public static async Task<IResult> VerifyAuthCode(
                        [FromBody] ValidationCodeDto request, IAuthService authService)
        {
            var result = await authService.VerifyAuthCode(request);

            if (!result.Success)
            {
                return TypedResults.Problem(detail: result.Message, statusCode: 400);
            }

            return TypedResults.Ok(new { message = result.Message, data = result });
        }

        public static async Task<IResult> ResetPassword(
            [FromBody] PasswordResetConfirmDto request, IAuthService authService)
        {
            var result = await authService.ResetPassword(request);

            if (!result.Success)
            {
                return TypedResults.Problem(detail: result.Message, statusCode: 400);
            }

            return TypedResults.Ok(new { message = result.Message, data = result });
        }

        public static async Task<IResult> GenerateToken(
            [FromBody] LoginCredentialsDto credentials, IAuthService authService)
        {
            string? token = await authService.GenerateTokenAsync(credentials);

            if (string.IsNullOrEmpty(token))
            {
                return TypedResults.Unauthorized();
            }

            return TypedResults.Ok(new { token });
        }

        public static async Task<IResult> Login(
            HttpContext httpContext,
            [FromBody] LoginCredentialsDto credentials, IAuthService authService)
        {
            string? token = await authService.GenerateTokenAsync(credentials);

            if (string.IsNullOrEmpty(token))
            {
                return TypedResults.Problem(detail: "Invalid credentials or email not verified", statusCode: 401);
            }

            httpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                Path = "/"
            });

            return TypedResults.Ok(new
            {
                tokenSetInCookie = true,
                message = "Logged in"
            });
        }

        public static IResult ValidateToken(ClaimsPrincipal user)
        {
            //If the code reaches here, it means the cookie is valid and the user is authenticated.

            return TypedResults.Ok(new MessageResponse("Token is valid."));
        }

        public static async Task<IResult> LogoutAsync(
            HttpContext httpContext,
            CancellationToken cancellationToken, IAuthService authService)
        {
            var user = httpContext.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                return TypedResults.Problem(detail: "User not authenticated", statusCode: 401);
            }

            var jti = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            var expString = user.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;

            if (!long.TryParse(expString, out var expSeconds))
            {
                return TypedResults.Problem(detail: "Claim exp not valid.", statusCode: 400);
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

            return TypedResults.Ok(new MessageResponse("Logout done, token revokated."));
        }

        public static async Task<IResult> VerifyEmail(
            [FromServices] IAuthService authService,
            [FromBody] VerifyEmailRequestDto request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Token))
            {
                return TypedResults.Problem(detail: "Email or token missing.", statusCode: 400);
            }

            var result = await authService.VerifyEmailAsync(request.Token, cancellationToken);

            if (!result)
            {
                return TypedResults.Problem(detail: "Verification failed.", statusCode: 400);
            }

            return TypedResults.Ok(new MessageResponse("Email successfully verified."));
        }
    }
}