using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.DTOs;
using nam.Server.Models.Entities;
using nam.Server.Models.Services.Infrastructure;
using System.IdentityModel.Tokens.Jwt;



namespace nam.Server.Endpoints
{
    internal static class AuthEndpoints
    {
        public static async Task<IResult> RegisterUser(
            [FromBody] RegisterUserDto request,
            ApplicationDbContext context,
            IValidator<RegisterUserDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var existingUser = await context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                return TypedResults.Conflict("Email is already in use.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash
            };

            context.Users.Add(newUser);
            await context.SaveChangesAsync();

            return TypedResults.Ok("User registered successfully");
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
