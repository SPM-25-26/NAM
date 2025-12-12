using nam.Server.Models.DTOs;
using nam.Server.Models.Services.Infrastructure.Services.Implemented.Auth;
using nam.ServerTests.mock;

namespace nam.ServerTests
{
    [TestClass]
    public sealed class AuthLoginTests
    {
        private AuthServiceTestBuilder _builder = null!;
        private AuthService _authService = null!;

        [TestInitialize]
        public void Setup()
        {
            _builder = new AuthServiceTestBuilder();
            _authService = _builder.Build();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _builder.Dispose();
        }

        [TestMethod]
        public async Task Login_ReturnsToken_WhenCredentialsAreValid()
        {
            // Arrange
            var email = "user@example.com";
            var passwordPlain = "$Password1";

            // Insert the user into the in-memory DB via the builder
            await _builder.SeedUserAsync(email, passwordPlain, isVerified: true);

            var credentials = new LoginCredentialsDto(email, passwordPlain);

            // Act - Call the service directly (pure unit test of the service)
            var token = await _authService.GenerateTokenAsync(credentials);

            // Assert
            Assert.IsNotNull(token, "Il token non dovrebbe essere null");
            Assert.AreEqual("fake-jwt-token-generated", token);
        }

        [TestMethod]
        public async Task Login_ReturnsNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var email = "user@example.com";
            await _builder.SeedUserAsync(email, "PasswordCorretta", isVerified: true);

            var credentials = new LoginCredentialsDto(email, "PasswordSbagliata!");

            // Act
            var token = await _authService.GenerateTokenAsync(credentials);

            // Assert
            Assert.IsNull(token, "Il token dovrebbe essere null se la password è errata");
        }

        [TestMethod]
        public async Task Login_ReturnsNull_WhenEmailNotVerified()
        {
            // Arrange
            var email = "notverified@example.com";
            // Creiamo l'utente ma con email NON verificata
            await _builder.SeedUserAsync(email, "Password123", isVerified: false);

            var credentials = new LoginCredentialsDto(email, "Password123");

            // Act
            var token = await _authService.GenerateTokenAsync(credentials);

            // Assert
            Assert.IsNull(token, "Il token dovrebbe essere null se l'email non è verificata");
        }

        [TestMethod]
        public async Task Login_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            // Nessun utente nel DB
            var credentials = new LoginCredentialsDto("ghost@example.com", "Password123");

            // Act
            var token = await _authService.GenerateTokenAsync(credentials);

            // Assert
            Assert.IsNull(token);
        }
    }
}