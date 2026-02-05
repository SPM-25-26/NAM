using NUnit.Framework;
using nam.ServerTests.Integration.Shared;

namespace nam.ServerTests.Integration.Api
{
    [TestFixture]
    public sealed class BasicReachabilityTests
    {
        private NamTestFactory? _factory;
        private HttpClient? _client;

        [SetUp]
        public void Setup()
        {
            _factory = new NamTestFactory();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        [Test]
        public async Task Health_endpoint_is_reachable_async()
        {
            var client = _client ?? throw new System.InvalidOperationException("HTTP client was not initialized.");

            var response = await client.GetAsync("/health");

            NUnit.Framework.Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }
    }
}
