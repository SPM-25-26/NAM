using DataInjection.Core.Interfaces;
using DataInjection.Core.Providers;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Core
{
    public sealed class ProviderTestDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public sealed class ProviderTestEntity
    {
        public string Name { get; set; } = string.Empty;
    }

    [TestFixture]
    public class ProviderBaseUrlTests
    {
        [Test]
        public void WebServerProvider_ReturnsServerHttpsValue()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "SERVER_HTTPS", "https://server.example.com" }
                })
                .Build();

            var fetcher = Substitute.For<IFetcher>();
            var mapper = Substitute.For<IDtoMapper<ProviderTestDto, ProviderTestEntity>>();

            var provider = new WebServerProvider<ProviderTestDto, ProviderTestEntity>(
                configuration,
                fetcher,
                mapper,
                "/api/test",
                new Dictionary<string, string?>());

            var result = provider.GetBaseUrl();

            NUnitAssert.That(result, Is.EqualTo("https://server.example.com"));
        }

        [Test]
        public void ExternalEndpointProvider_ReturnsDataInjectionApiValue()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "DataInjectionApi", "https://api.example.com" }
                })
                .Build();

            var fetcher = Substitute.For<IFetcher>();
            var mapper = Substitute.For<IDtoMapper<ProviderTestDto, ProviderTestEntity>>();

            var provider = new ExternalEndpointProvider<ProviderTestDto, ProviderTestEntity>(
                configuration,
                fetcher,
                mapper,
                "/api/test",
                new Dictionary<string, string?>());

            var result = provider.GetBaseUrl();

            NUnitAssert.That(result, Is.EqualTo("https://api.example.com"));
        }
    }
}
