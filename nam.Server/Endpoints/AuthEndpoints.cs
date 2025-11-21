using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.DTOs;
using nam.Server.Models.Entities;
using System;

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
    }
}
