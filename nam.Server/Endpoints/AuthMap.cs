using nam.Server.Models.DTOs;
using nam.Server.Models.Swagger;

namespace nam.Server.Endpoints
{
    internal static class AuthMap
    {
        public static IEndpointRouteBuilder MapAuth(this IEndpointRouteBuilder builder)
        {

            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            AuthEndpoints.ConfigureLogger(logger);

            RouteGroupBuilder groupBuilder = builder.MapGroup("/api/auth")
            .RequireCors("FrontendWithCredentials")
            .WithTags("Authentication");

            // POST /api/auth/password-reset/register
            groupBuilder.MapPost("/register", AuthEndpoints.RegisterUser)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("User registration")
                .WithDescription("Registers a new user in the system with the provided details.");

            // POST /api/auth/password-reset
            groupBuilder.MapPost("/password-reset", AuthEndpoints.ResetPassword)
                .Produces<PasswordResetResponseDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Reset password")
                .WithDescription("Resets the user's password using the authentication code and new password.");

            // POST /api/auth/password-reset/request
            groupBuilder.MapPost("/password-reset/request", AuthEndpoints.RequestPasswordReset)
                .Produces<PasswordResetResponseDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Request Reset password")
                .WithDescription("Sends a 6-digit authentication code to the user's email if the account exists.");

            // POST /api/auth/password-reset/verify
            groupBuilder.MapPost("/password-reset/verify", AuthEndpoints.VerifyAuthCode)
                .Produces<PasswordResetResponseDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Verify password reset code")
                .WithDescription("Verifies the 6-digit code sent to the user's email during the password reset process.");

            // POST /api/auth/generate-token (for swagger, token string)
            groupBuilder.MapPost("/generate-token", AuthEndpoints.GenerateToken)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("JWT generation.");

            // POST /api/auth/login (cookie)
            groupBuilder.MapPost("/login", AuthEndpoints.Login)
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status401Unauthorized)
           .Produces(StatusCodes.Status500InternalServerError)
           .WithSummary("User login")
           .WithDescription("Authenticates the user with email/username and password. Returns a JWT access token if successful.");

            // POST /api/auth/logout
            groupBuilder.MapPost("/logout", AuthEndpoints.LogoutAsync)
                .RequireAuthorization()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .WithSummary("User logout")
                .WithDescription("Close session. Revokes the JWT")
                .WithOpenApi((op) =>
                {
                    op.Security = ApiSecurityDocSwagger.BearerRequirement;
                    return op;
                });

            // POST /api/auth/verify-email
            groupBuilder.MapPost("/verify-email", AuthEndpoints.VerifyEmail)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi(op =>
            {
                op.Summary = "Verify user email.";
                op.Description = "Verifies the user's email using the verification token sent by email.";
                return op;
            });

            return builder;
        }
    }
}
