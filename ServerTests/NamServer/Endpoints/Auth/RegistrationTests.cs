using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using nam.Server.ApiResponse;
using nam.Server.DTOs;
using nam.Server.Endpoints.Auth;
using nam.Server.Services.Interfaces.Auth;
using nam.Server.Validators;
using nam.ServerTests.NamServer.Endpoints.Auth.mock;
using NUnit.Framework;
using Serilog;
using Assert = NUnit.Framework.Assert;

namespace nam.ServerTests.NamServer.Endpoints.Auth
{
    [TestFixture]
    public sealed class RegistrationTests
    {
        private AuthServiceTestBuilder _builder = null!;
        private IAuthService _authService = null!;

        [SetUp]
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

        [TearDown]
        public void Cleanup()
        {
            _builder.Dispose();
            Log.CloseAndFlush();
        }

        [Test]
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

            Assert.That(result, Is.InstanceOf<Ok<MessageResponse>>());

            var userInDb = await _builder.Context.Users.FirstOrDefaultAsync(u => u.Email == registrationData.Email);

            Assert.That(userInDb, Is.Not.Null);
            Assert.That(userInDb!.Email, Is.EqualTo("validmail@gmail.com"));
            Assert.That(userInDb.PasswordHash, Is.Not.EqualTo("ValidPassword123!"));
        }

        [Test]
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

            Assert.That(result, Is.InstanceOf<Conflict<MessageResponse>>());
        }

        [Test]
        public async Task RegisterUser_ReturnsValidationProblem_WhenPasswordsDoNotMatch()
        {
            RegisterUserValidator validator = new();
            RegisterUserDto registrationData = new()
            {
                Email = "newuser@example.com",
                Password = "ValidPasswordA123!",
                ConfirmPassword = "ValidPasswordB123!"
            };

            var result = await AuthEndpoints.RegisterUser(registrationData, _authService, validator);

            Assert.That(result, Is.InstanceOf<ValidationProblem>());
        }

        [Test]
        public async Task RegisterUser_ReturnsValidationProblem_WhenEmailIsInvalid()
        {
            RegisterUserValidator validator = new();
            RegisterUserDto registrationData = new()
            {
                Email = "not-an-email",
                Password = "ValidPassword123!",
                ConfirmPassword = "ValidPassword123!"
            };

            var result = await AuthEndpoints.RegisterUser(registrationData, _authService, validator);

            Assert.That(result, Is.InstanceOf<ValidationProblem>());
        }

        [Test]
        public async Task RegisterUser_ReturnsValidationProblem_WhenPasswordIsTooWeak()
        {
            RegisterUserValidator validator = new();
            RegisterUserDto registrationData = new()
            {
                Email = "valid@example.com",
                Password = "123",
                ConfirmPassword = "123"
            };

            var result = await AuthEndpoints.RegisterUser(registrationData, _authService, validator);

            Assert.That(result, Is.InstanceOf<ValidationProblem>());
        }
    }
}