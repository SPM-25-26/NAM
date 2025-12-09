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
    public sealed class RegistrationTests
    {
        private AuthServiceTestBuilder _builder = null!;
        private IAuthService _authService = null!;

        [TestInitialize]
        public void Setup()
        {
            // 1. Use the Builder to configure in-memory DB and fake dependencies
            _builder = new AuthServiceTestBuilder();
            _authService = _builder.Build();

            // Configure Serilog
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
            // Arrange
            RegisterUserValidator validator = new();
            RegisterUserDto registrationData = new()
            {
                Email = "validmail@gmail.com",
                Password = "ValidPassword123!",
                ConfirmPassword = "ValidPassword123!"
            };

            // Act
            // Assume that the endpoint now accepts IAuthService
            var result = await AuthEndpoints.RegisterUser(registrationData, _authService, validator);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Ok<MessageResponse>));

            // Verify in the DB (via the builder's context)
            var userInDb = await _builder.Context.Users.FirstOrDefaultAsync(u => u.Email == registrationData.Email);

            Assert.IsNotNull(userInDb, "User should exist in the database");
            Assert.AreEqual("validmail@gmail.com", userInDb.Email);

            // Optional verification: password hashed?
            Assert.AreNotEqual("ValidPassword123!", userInDb.PasswordHash);
        }

        [TestMethod]
        public async Task RegisterUser_ReturnsConflict_WhenEmailAlreadyExists()
        {
            // Arrange
            RegisterUserValidator validator = new();
            var existingEmail = "existing@example.com";

            // Seed of the DB using the builder's helper method
            await _builder.SeedUserAsync(existingEmail, "SomePassword123!");

            RegisterUserDto registrationData = new()
            {
                Email = existingEmail,
                Password = "ValidPassword123!",
                ConfirmPassword = "ValidPassword123!"
            };

            // Act
            var result = await AuthEndpoints.RegisterUser(registrationData, _authService, validator);

            // Assert
            // Assume that AuthService returns false if exists, and the endpoint returns Conflict
            Assert.IsInstanceOfType(result, typeof(Conflict<MessageResponse>));
        }

        [TestMethod]
        public async Task RegisterUser_ReturnsValidationProblem_WhenPasswordsDoNotMatch()
        {
            // Arrange
            RegisterUserValidator validator = new();
            RegisterUserDto registrationData = new()
            {
                Email = "newuser@example.com",
                Password = "ValidPasswordA123!",
                ConfirmPassword = "ValidPasswordB123!" // Mismatch
            };

            // Act
            // The validator triggers before calling the service, so _authService will not actually be invoked
            var result = await AuthEndpoints.RegisterUser(registrationData, _authService, validator);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ValidationProblem));
        }

        [TestMethod]
        public async Task RegisterUser_ReturnsValidationProblem_WhenEmailIsInvalid()
        {
            // Arrange
            RegisterUserValidator validator = new();
            RegisterUserDto registrationData = new()
            {
                Email = "not-an-email",
                Password = "ValidPassword123!",
                ConfirmPassword = "ValidPassword123!"
            };

            // Act
            var result = await AuthEndpoints.RegisterUser(registrationData, _authService, validator);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ValidationProblem));
        }

        [TestMethod]
        public async Task RegisterUser_ReturnsValidationProblem_WhenPasswordIsTooWeak()
        {
            // Arrange
            RegisterUserValidator validator = new();
            RegisterUserDto registrationData = new()
            {
                Email = "valid@example.com",
                Password = "123", // Too short/simple
                ConfirmPassword = "123"
            };

            // Act
            var result = await AuthEndpoints.RegisterUser(registrationData, _authService, validator);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ValidationProblem));
        }
    }
}