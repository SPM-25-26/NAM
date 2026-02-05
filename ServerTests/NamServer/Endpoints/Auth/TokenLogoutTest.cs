using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using nam.Server.Endpoints.Auth;
using nam.Server.Services.Interfaces.Auth;
using nam.ServerTests.NamServer.Endpoints.Auth.mock;
using NUnit.Framework;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Assert = NUnit.Framework.Assert;

namespace nam.ServerTests.NamServer.Endpoints.Auth
{
    [TestFixture]
    public sealed class TokenLogoutTest
    {
        private AuthServiceTestBuilder _builder = null!;
        private IAuthService _authService = null!;

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
        public async Task Logout_ReturnsOk_AndRevokesToken_WhenUserIsAuthenticated()
        {
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

            var result = await AuthEndpoints.LogoutAsync(httpContext, CancellationToken.None, _authService);

            Assert.That(result, Is.Not.InstanceOf<UnauthorizedHttpResult>());
            Assert.That(result, Is.Not.InstanceOf<BadRequest<string>>());

            var okResult = result as dynamic;
            Assert.That(okResult, Is.Not.Null);

            Assert.That(okResult.Value, Is.Not.Null);
            Assert.That(okResult.Value.Message, Is.EqualTo("Logout done, token revokated."));

            var isRevokedInDb = await _builder.Context.RevokedTokens.AnyAsync(t => t.Jti == jti);
            Assert.That(isRevokedInDb, Is.True);
        }

        [Test]
        public async Task Logout_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            var result = await AuthEndpoints.LogoutAsync(httpContext, CancellationToken.None, _authService);

            Assert.That(result, Is.InstanceOf<ProblemHttpResult>());
        }
    }
}