using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using nam.Server.Endpoints;
using nam.Server.Services.Interfaces.RecSys;
using NSubstitute;
using NUnit.Framework;
using Serilog;
using System.Security.Claims;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.NamServer.Endpoints
{
    [TestFixture]
    public class RecEndpointsTests
    {
        [SetUp]
        public void SetUp()
        {
            // RecEndpoints richiede la configurazione esplicita del logger (static)
            RecEndpoints.ConfigureLogger(Substitute.For<ILogger>());
        }

        [Test]
        public void GetRec_ThrowsInvalidOperation_WhenLoggerNotConfigured()
        {
            // Arrange: reset static logger via reflection per testare il comportamento
            var loggerField = typeof(RecEndpoints)
                .GetField("_logger", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            loggerField!.SetValue(null, null);

            var service = Substitute.For<IRecSysService>();
            var ctx = new DefaultHttpContext();

            // Act + Assert
            NUnitAssert.ThrowsAsync<InvalidOperationException>(async () =>
                await RecEndpoints.GetRec(service, ctx, null, null, CancellationToken.None));
        }

        [Test]
        public async Task GetRec_ReturnsUnauthorized_WhenEmailClaimMissing()
        {
            var service = Substitute.For<IRecSysService>();
            var ctx = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity([]))
            };

            var result = await RecEndpoints.GetRec(service, ctx, null, null, CancellationToken.None);

            NUnitAssert.That(result, Is.InstanceOf<UnauthorizedHttpResult>());
            await service.DidNotReceiveWithAnyArgs().GetRecommendationsAsync(default!, default, default);
        }

        [Test]
        public async Task GetRec_ReturnsOk_AndCallsServiceWithNullLocation_WhenLatLonMissing()
        {
            var service = Substitute.For<IRecSysService>();
            var expected = new List<string> { "a", "b" };

            service.GetRecommendationsAsync("user@test.it", null, null)
                .Returns(expected);

            var ctx = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(
                    [new Claim(ClaimTypes.Email, "user@test.it")],
                    authenticationType: "TestAuth"))
            };

            var result = await RecEndpoints.GetRec(service, ctx, null, null, CancellationToken.None);

            var ok = result as Ok<List<string>>;
            NUnitAssert.That(ok, Is.Not.Null);
            NUnitAssert.That(ok!.Value, Is.SameAs(expected));

            await service.Received(1).GetRecommendationsAsync("user@test.it", null, null);
        }

        [Test]
        public async Task GetRec_ReturnsOk_AndCallsServiceWithLocation_WhenLatLonProvided()
        {
            var service = Substitute.For<IRecSysService>();
            var expected = new List<string> { "x", "y" };

            service.GetRecommendationsAsync("user@test.it", 45.1, 9.2)
                .Returns(expected);

            var ctx = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(
                    [new Claim(ClaimTypes.Email, "user@test.it")],
                    authenticationType: "TestAuth"))
            };

            var result = await RecEndpoints.GetRec(service, ctx, 45.1, 9.2, CancellationToken.None);

            var ok = result as Ok<List<string>>;
            NUnitAssert.That(ok, Is.Not.Null);
            NUnitAssert.That(ok!.Value, Is.SameAs(expected));

            await service.Received(1).GetRecommendationsAsync("user@test.it", 45.1, 9.2);
        }
    }
}