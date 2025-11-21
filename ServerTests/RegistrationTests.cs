using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Endpoints;
using nam.Server.Models.DTOs;
using nam.Server.Models.Entities;
using nam.Server.Models.Validators;

namespace nam.ServerTests
{
    [TestClass]
    public sealed class RegistrationTests
    {
        private ApplicationDbContext context;

        [TestInitialize]
        public void Setup()
        {
            // This creates the server in-memory
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            // 2. Create the Context
            context = new ApplicationDbContext(options);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
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

            var result = await AuthEndpoints.RegisterUser(registrationData, context, validator);

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
            var result = await AuthEndpoints.RegisterUser(registrationData, context, validator);

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
                Password = "PasswordA",
                ConfirmPassword = "PasswordB" // Mismatch
            };

            // Act
            var result = await AuthEndpoints.RegisterUser(registrationData, context, validator);

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
            var result = await AuthEndpoints.RegisterUser(registrationData, context, validator);

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
            var result = await AuthEndpoints.RegisterUser(registrationData, context, validator);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ValidationProblem));
        }    
}
}

