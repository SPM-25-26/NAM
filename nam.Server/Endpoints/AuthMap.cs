using Microsoft.OpenApi.Models;
using nam.Server.Models.DTOs;

namespace nam.Server.Endpoints
{
    internal static class AuthMap
    {
        public static IEndpointRouteBuilder MapAuth(this IEndpointRouteBuilder builder)
        {

            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            AuthEndpoints.ConfigureLogger(logger);

            RouteGroupBuilder groupBuilder = builder.MapGroup("/api/auth").RequireCors("FrontendWithCredentials");

            groupBuilder.MapPost("/register", AuthEndpoints.RegisterUser)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithOpenApi(op =>
                {
                    op.Summary = "User registration.";
                    return op;
                });

            groupBuilder.MapPost("/request-password-reset", AuthEndpoints.RequestPasswordReset)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithOpenApi(op =>
                {
                    op.Summary = "Requests a password reset code for a given email address.";
                    op.Description = "Sends a 6-digit code to the user's email if the account exists.";
                    return op;
                });

            groupBuilder.MapPost("/password-reset", AuthEndpoints.ResetPassword)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithOpenApi(op =>
                {
                    op.Summary = "Resets the user's password using the Auth Code sent to their email.";
                    op.Description = "Requires the user's email, the Auth Code, and the new password.";
                    return op;
                });
            groupBuilder.MapPost("/request-password-reset/verify-code", AuthEndpoints.VerifyAuthCode)
                .Produces<PasswordResetResponseDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithOpenApi(op =>
               {
                   op.Summary = "Verify password reset authentication code";
                   op.Description = "Verifies the 6-digit authentication code sent to the user's email during a password reset process.";
                   op.Tags = [new OpenApiTag { Name = "Authentication" }];
                   return op;
               });
            // POST /api/auth/generate-token (for swagger, token string)
            groupBuilder.MapPost("/generate-token", AuthEndpoints.GenerateToken)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithOpenApi(op =>
                {
                    op.Summary = "JWT generation.";
                    return op;
                });

            // POST /api/auth/login (cookie)
            groupBuilder.MapPost("/login", AuthEndpoints.Login)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithOpenApi(op =>
                {
                    op.Summary = "JWT generation.";
                    return op;
                });

            // POST /api/auth/logout
            groupBuilder.MapPost("/logout", AuthEndpoints.LogoutAsync)
                .RequireAuthorization()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .WithOpenApi(op =>
                {
                    op.Summary = "User logout (token revocation).";
                    op.Description = "Revokes the current JWT access token by adding its jti to the blacklist.";
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
