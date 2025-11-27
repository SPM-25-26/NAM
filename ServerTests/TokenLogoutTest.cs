using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using nam.Server.Endpoints;
using nam.Server.Models.Services.Infrastructure;

namespace nam.ServerTests
{
    [TestClass]
    public sealed class LogoutTests
    {
        private FakeTokenService _tokenService = null!;

        [TestInitialize]
        public void Setup()
        {
            _tokenService = new FakeTokenService();
        }

        [TestMethod]
        public async Task Logout_ReturnsOk_AndRevokesToken_WhenUserIsAuthenticated()
        {
            // Arrange: HttpContext with authenticated user and claim jti/exp
            var httpContext = new DefaultHttpContext();

            var jti = Guid.NewGuid().ToString();
            var exp = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds().ToString();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(JwtRegisteredClaimNames.Exp, exp),
            };

            var identity = new ClaimsIdentity(claims, authenticationType: "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            httpContext.User = principal;

            // Act
            var result = await AuthEndpoints.LogoutAsync(httpContext, _tokenService, CancellationToken.None);

            Assert.IsNotInstanceOfType(result, typeof(UnauthorizedHttpResult));
            Assert.IsNotInstanceOfType(result, typeof(BadRequest<string>));

            var okResult = result as dynamic;
            Assert.IsNotNull(okResult);

            var value = okResult.Value;
            Assert.IsNotNull(value);

            string message = value.message;
            Assert.AreEqual("Logout done, token revokated.", message);

            Assert.IsTrue(
                _tokenService.RevokedTokens.Contains(jti),
                "Il token (jti) dovrebbe essere stato registrato come revocato.");
        }

        [TestMethod]
        public async Task Logout_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange: HttpContext user not authenticated
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity()); 

            // Act
            var result = await AuthEndpoints.LogoutAsync(httpContext, _tokenService, CancellationToken.None);

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedHttpResult));
        }


        // Fake ITokenService for tests: it keep in memory revokated jti.
        private sealed class FakeTokenService : ITokenService
        {
            public HashSet<string> RevokedTokens { get; } = new();

            public Task RevokeTokenAsync(string jti, DateTime expiresAt, CancellationToken cancellationToken = default)
            {
                if (!string.IsNullOrEmpty(jti))
                {
                    RevokedTokens.Add(jti);
                }
                return Task.CompletedTask;
            }

            public Task<bool> IsTokenRevokedAsync(string jti, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(RevokedTokens.Contains(jti));
            }

            //stub
            public Task<int> CleanupExpiredRevokedTokensAsync(CancellationToken cancellationToken = default)
                 => Task.FromResult(0);
        }
    }
}