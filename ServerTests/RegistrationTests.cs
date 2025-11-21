using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Endpoints;
using nam.Server.Models.DTOs;
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
    }
}

