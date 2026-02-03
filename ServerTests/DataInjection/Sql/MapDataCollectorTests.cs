using DataInjection.Core.Interfaces;
using DataInjection.SQL.Collectors;
using DataInjection.SQL.DTOs;
using Domain.Entities.MunicipalityEntities;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class MapDataCollectorTests
    {
        [Test]
        public async Task GetEntities_SetsMunicipalityNameOnEntity()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "DataInjectionApi", "https://api.example.com" }
                })
                .Build();

            var fetcher = Substitute.For<IFetcher>();
            fetcher.Fetch<MapDataDto>(
                    Arg.Any<string>(),
                    "api/map",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new MapDataDto
                {
                    CenterLatitude = 1.2,
                    CenterLongitude = 3.4
                });

            var collector = new MapDataCollector(fetcher, configuration);

            var result = await collector.GetEntities("Milano");

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Name, Is.EqualTo("Milano"));
            NUnitAssert.That(result[0].CenterLatitude, Is.EqualTo(1.2));
            NUnitAssert.That(result[0].CenterLongitude, Is.EqualTo(3.4));
        }
    }
}
