using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using nam.Server.Endpoints;
using nam.Server.Models.ApiResponse;
using nam.Server.Models.DTOs;
using nam.Server.Models.Services.Infrastructure;
using nam.Server.Models.Validators;
using nam.ServerTests.mock;
using Serilog;

namespace nam.ServerTests
{
    [TestClass]
    public sealed class ResetPasswordTests
    {

        private AuthServiceTestBuilder _builder = null!;
        private IAuthService _authService = null!;

        [TestInitialize]
        public void Setup()
        {
            _builder = new AuthServiceTestBuilder();
            _authService = _builder.Build();

            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            AuthEndpoints.ConfigureLogger(logger);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _builder.Dispose();
            Log.CloseAndFlush();
        }

        [TestMethod]
        public async Task RegisterUser_ReturnsOk_WhenRegistrationIsSuccessfulAsync()
        {
            RegisterUserValidator validator = new();
            RegisterUserDto registrationData = new()
            {
                Email = "validmail@gmail.com",
                Password = "ValidPassword123!",
                ConfirmPassword = "ValidPassword123!"
            };

            var result = await AuthEndpoints.RegisterUser(registrationData, _authService, validator);

            Assert.IsInstanceOfType(result, typeof(Ok<MessageResponse>));

            var userInDb = await _builder.Context.Users.FirstOrDefaultAsync(u => u.Email == registrationData.Email);

            Assert.IsNotNull(userInDb, "User should exist in the database");
            Assert.AreEqual("validmail@gmail.com", userInDb.Email);

            Assert.AreNotEqual("ValidPassword123!", userInDb.PasswordHash);
        }

        [TestMethod]
        public async Task RegisterUser_ReturnsConflict_WhenEmailAlreadyExists()
        {
            RegisterUserValidator validator = new();
            var existingEmail = "existing@example.com";

            await _builder.SeedUserAsync(existingEmail, "SomePassword123!");

            RegisterUserDto registrationData = new()
            {
                Email = existingEmail,
                Password = "ValidPassword123!",
                ConfirmPassword = "ValidPassword123!"
            };

            var result = await AuthEndpoints.RegisterUser(registrationData, _authService, validator);

            Assert.IsInstanceOfType(result, typeof(Conflict<MessageResponse>));
        }

        [TestMethod]
        public async Task RegisterUser_ReturnsValidationProblem_WhenPasswordsDoNotMatch()
        {
            RegisterUserValidator validator = new();
            RegisterUserDto registrationData = new()
            {
                Email = "newuser@example.com",
                Password = "ValidPasswordA123! ",
                ConfirmPassword = "ValidPasswordB123!"
            };

            var result = await AuthEndpoints.RegisterUser(registrationData, _authService, validator);

            Assert.IsInstanceOfType(result, typeof(ValidationProblem));
        }
    }
}