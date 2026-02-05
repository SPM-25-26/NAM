using System.Net.Http;
using nam.ServerTests.Integration.Shared;

namespace nam.ServerTests.Integration.Api;

[TestClass]
public sealed class BasicReachabilityTests
{
    private NamTestFactory? _factory;
    private HttpClient? _client;

    [TestInitialize]
    public void Setup()
    {
        _factory = new NamTestFactory();
        _client = _factory.CreateClient();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [TestMethod]
    public async Task Health_endpoint_is_reachable_async()
    {
        var client = _client ?? throw new InvalidOperationException("HTTP client was not initialized.");
        var response = await client.GetAsync("/health");

        response.EnsureSuccessStatusCode();
    }
}
