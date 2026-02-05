using nam.Server.DTOs;
using nam.Server.Services.Implemented.Auth;
using nam.ServerTests.NamServer.Endpoints.Auth.mock;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace nam.ServerTests.NamServer.Endpoints.Auth
{
    [TestFixture]
    public sealed class AuthLoginTests
    {
        private AuthServiceTestBuilder _builder = null!;
        private AuthService _authService = null!;

        [SetUp]
        public void Setup()
        {
            _builder = new AuthServiceTestBuilder();
            _authService = _builder.Build();
        }

        [TearDown]
        public void Cleanup()
        {
            _builder.Dispose();
        }

        [Test]
        public async Task Login_ReturnsToken_WhenCredentialsAreValid()
        {
            var email = "user@example.com";
            var passwordPlain = "$Password1";

            await _builder.SeedUserAsync(email, passwordPlain, isVerified: true);

            var credentials = new LoginCredentialsDto(email, passwordPlain);

            var token = await _authService.GenerateTokenAsync(credentials);

            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.EqualTo("fake-jwt-token-generated"));
        }

        [Test]
        public async Task Login_ReturnsNull_WhenCredentialsAreInvalid()
        {
            var email = "user@example.com";
            await _builder.SeedUserAsync(email, "PasswordCorretta", isVerified: true);

            var credentials = new LoginCredentialsDto(email, "PasswordSbagliata!");

            var token = await _authService.GenerateTokenAsync(credentials);

            Assert.That(token, Is.Null);
        }

        [Test]
        public async Task Login_ReturnsNull_WhenEmailNotVerified()
        {
            var email = "notverified@example.com";
            await _builder.SeedUserAsync(email, "Password123", isVerified: false);

            var credentials = new LoginCredentialsDto(email, "Password123");

            var token = await _authService.GenerateTokenAsync(credentials);

            Assert.That(token, Is.Null);
        }

        [Test]
        public async Task Login_ReturnsNull_WhenUserDoesNotExist()
        {
            var credentials = new LoginCredentialsDto("ghost@example.com", "Password123");

            var token = await _authService.GenerateTokenAsync(credentials);

            Assert.That(token, Is.Null);
        }
    }
}