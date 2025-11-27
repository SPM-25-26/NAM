using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using nam.Server.Data;                
using nam.Server.Endpoints;            
using nam.Server.Models.DTOs;
using nam.Server.Models.Entities;
using nam.Server.Models.Services.Infrastructure;

namespace nam.ServerTests
{
    [TestClass]
    public sealed class LoginTests
    {
        private ApplicationDbContext _context = null!;
        private IAuthService _authService = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"LoginTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);

            _authService = new FakeAuthService(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task Login_ReturnsOk_WithToken_WhenCredentialsAreValid()
        {
            var email = "user@example.com";
            var passwordPlain = "$Password1";

            var user = new User
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordPlain)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var credentials = new LoginCredentialsDto(email, passwordPlain);

            // Act
            var result = await AuthEndpoints.GenerateToken(_authService, credentials);

            // Assert
            Assert.IsNotInstanceOfType(result, typeof(UnauthorizedHttpResult));

            var okResult = result as dynamic;
            Assert.IsNotNull(okResult);

            var value = okResult.Value;
            Assert.IsNotNull(value);

            string token = value.token;
            Assert.IsFalse(string.IsNullOrWhiteSpace(token));
        }

        [TestMethod]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var credentials = new LoginCredentialsDto("nonexistent@example.com", "WrongPassword123!");

            // Act
            var result = await AuthEndpoints.GenerateToken(_authService, credentials);

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedHttpResult));
        }

        private sealed class LoginResponseDto
        {
            public string Token { get; set; } = string.Empty;
        }

        // Fake IAuthService for tests
        private sealed class FakeAuthService : IAuthService
        {
            private readonly ApplicationDbContext _context;

            public FakeAuthService(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<string?> GenerateTokenAsync(
                LoginCredentialsDto credentials,
                CancellationToken cancellationToken = default)
            {
                var user = await _context.Users.FirstOrDefaultAsync(
                    u => u.Email == credentials.Email, cancellationToken);

                if (user is null)
                    return null;

                var valid = BCrypt.Net.BCrypt.Verify(credentials.Password, user.PasswordHash);
                if (!valid)
                    return null;

                return "fake-jwt-token";
            }
        }
    }
}