using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Endpoints;
using nam.Server.Models.DTOs;
using nam.Server.Models.Entities;
using nam.Server.Models.Services.Infrastructure.Repositories;
using nam.Server.Models.Validators;
using Serilog;

namespace nam.ServerTests
{
    [TestClass]
    public sealed class RegistrationTests
    {
        private ApplicationDbContext context;
        private UserRepository userRepository;

        [TestInitialize]
        public void Setup()
        {
            // This creates the server in-memory
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            // 2. Create the Context and repository
            context = new ApplicationDbContext(options);
            userRepository = new UserRepository(context);

            // Configure a Serilog logger for the endpoints
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            AuthEndpoints.ConfigureLogger(logger);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Ensure DB cleaned up
            context.Database.EnsureDeleted();
            context.Dispose();

            // Flush/close Serilog used by tests
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

            var result = await AuthEndpoints.RegisterUser(registrationData, userRepository, validator);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Ok<string>));

            var userInDb = await context.Users.FirstOrDefaultAsync(u => u.Email == registrationData.Email);

            Assert.IsNotNull(userInDb, "User should exist in the database");
            Assert.AreEqual("validmail@gmail.com", userInDb.Email);
        }

        [TestMethod]
        public async Task RegisterUser_ReturnsConflict_WhenEmailAlreadyExists()
        {
            // Arrange
            RegisterUserValidator validator = new();
            var existingEmail = "existing@example.com";

            // Seed the database with an existing user
            context.Users.Add(new User
            {
                Email = existingEmail,
                PasswordHash = "SomeHash"
            });
            await context.SaveChangesAsync();

            RegisterUserDto registrationData = new()
            {
                Email = existingEmail,
                Password = "ValidPassword123!",
                ConfirmPassword = "ValidPassword123!"
            };

            // Act
            var result = await AuthEndpoints.RegisterUser(registrationData, userRepository, validator);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Conflict<string>));
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
            var result = await AuthEndpoints.RegisterUser(registrationData, userRepository, validator);

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
            var result = await AuthEndpoints.RegisterUser(registrationData, userRepository, validator);

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
            var result = await AuthEndpoints.RegisterUser(registrationData, userRepository, validator);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ValidationProblem));
        }
    }
}

