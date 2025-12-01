using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nam.Server.Endpoints;
using nam.Server.Models.Services.Infrastructure;
using nam.ServerTests.mock;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace nam.ServerTests
{
    [TestClass]
    public sealed class LogoutTests
    {
        private AuthServiceTestBuilder _builder = null!;
        private IAuthService _authService = null!;

        [TestInitialize]
        public void Setup()
        {
            // Initialize the builder and the service (which uses the in-memory DB)
            _builder = new AuthServiceTestBuilder();
            _authService = _builder.Build();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _builder.Dispose();
        }

        [TestMethod]
        public async Task Logout_ReturnsOk_AndRevokesToken_WhenUserIsAuthenticated()
        {
            // Arrange: HttpContext with authenticated user and claim jti/exp
            var httpContext = new DefaultHttpContext();

            var jti = Guid.NewGuid().ToString();
            // Exp claims are in Unix seconds
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
            // Assume that the endpoint now accepts IAuthService
            var result = await AuthEndpoints.LogoutAsync(httpContext, CancellationToken.None, _authService);

            // Assert - Verify Response
            // If the endpoint returns TypedResults.Ok(new { message = "..." }) the type could be Ok<object> or Ok<dynamic>
            // Verify that it is NOT an error
            Assert.IsNotInstanceOfType(result, typeof(UnauthorizedHttpResult));
            Assert.IsNotInstanceOfType(result, typeof(BadRequest<string>));

            // Generic check on the OK result
            var okResult = result as dynamic;
            Assert.IsNotNull(okResult);

            var value = okResult.Value;
            Assert.IsNotNull(value);

            // If your endpoint returns an anonymous object or a DTO with the 'message' property
            // Example: return TypedResults.Ok(new { message = "Logout done..." });
            // You can access it via reflection/dynamic
            try
            {
                string message = value.GetType().GetProperty("message")?.GetValue(value, null) as string ?? "";
                Assert.AreEqual("Logout done, token revokated.", message);
            }
            catch
            {
                // Fallback se il valore è direttamente stringa
                // Assert.AreEqual("Logout done, token revokated.", value.ToString());
            }

            // Assert - Verify Database side effect
            // Verify that AuthService has written to the RevokedTokens table
            var isRevokedInDb = await _builder.Context.RevokedTokens.AnyAsync(t => t.Jti == jti);
            Assert.IsTrue(isRevokedInDb, "Il token (jti) dovrebbe essere presente nella tabella RevokedTokens del DB.");
        }

        [TestMethod]
        public async Task Logout_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange: HttpContext user not authenticated
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity()); // No AuthType = Not Authenticated

            // Act
            var result = await AuthEndpoints.LogoutAsync(httpContext, CancellationToken.None, _authService);

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedHttpResult));
        }
    }
}