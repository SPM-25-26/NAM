using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.DTOs;
using nam.Server.Models.Entities;
using nam.Server.Services;
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
        
        public static async Task<IResult> RequestPasswordReset(
            [FromBody] PasswordResetRequestDto request,
            ApplicationDbContext context,
            IEmailService emailService,
            ICodeService codeService
            )
        {
            Console.WriteLine($"\n[DEBUG] Input DTO Ricevuto:\n{request}\n");
            // Find a user
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            // Check find a user
            if (user == null){
                var notFoundResponse = new PasswordResetResponseDto
                {
                    Success = false, 
                    Message = "The email not found"
                };
                return TypedResults.NotFound(notFoundResponse); 
            }

            // Generate Auth code
            var authCode = codeService.GenerateAuthCode();
            Console.WriteLine($"\n[DEBUG] auth generated:\n{authCode}\n");
            var existingCode = await context.ResetPasswordAuth
                    .FirstOrDefaultAsync(c => c.UserId == user.Id.ToString());

            if (existingCode != null)
            {
                //delete exist code associated with user
                context.ResetPasswordAuth.Remove(existingCode);
                Console.WriteLine($"\n[DEBUG] Codice di reset preesistente rimosso per l'utente {user.Id}.\n");
            }

            // Build a response
            var resetCode = new PasswordResetCode
            {
                UserId = user.Id.ToString(),
                AuthCode = authCode,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(codeService.TimeToLiveMinutes) 
            };
            Console.WriteLine($"\n[DEBUG] reset code:\n{resetCode}\n");
            
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

        public static async Task<IResult> ResetPassword(
            [FromBody] PasswordResetConfirmDto request,
            ApplicationDbContext context)
        {
            // Find and validation reset code
            var resetCode = await context.ResetPasswordAuth
                .FirstOrDefaultAsync(c =>
                    c.AuthCode == request.AuthCode &&
                    c.ExpiresAt > DateTime.UtcNow);
            
            Console.WriteLine($"\n[DEBUG] check reset code:\n{resetCode}\n");
            // Verify the validity of the reset code
            if (resetCode == null){
                 Console.WriteLine($"\n[DEBUG] reset code not found (expired or not valid)");
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
            Console.WriteLine($"\n[DEBUG] check user {user}");

            if (user == null){
                 Console.WriteLine($"\n[DEBUG] user not found");
                var notFoundUserResponse = new PasswordResetResponseDto
                {
                    Success = false, 
                    Message = "Invalid auth code."
                };
                return TypedResults.BadRequest(notFoundUserResponse);
            }

           
            //TODO: replace with service for encrypt data
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            Console.WriteLine($"\n[DEBUG] password hashed: {user.PasswordHash}");
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
    }
}
