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
    public class ServiceCollectorTests
    {
        [Test]
        public async Task GetEntities_ReturnsEmpty_WhenNoCards()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "DataInjectionApi", "https://api.example.com" }
                })
                .Build();

            var fetcher = Substitute.For<IFetcher>();
            fetcher.Fetch<List<ServiceCardDto>>(
                    Arg.Any<string>(),
                    "api/services/card-list",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<ServiceCardDto>());

            var collector = new ServiceCollector(fetcher, configuration);

            var result = await collector.GetEntities("Milano");

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetEntities_LinksDetailAndAlignsIdentifier()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "DataInjectionApi", "https://api.example.com" }
                })
                .Build();

            var cardId = Guid.Parse("cccccccc-1111-2222-3333-dddddddddddd");
            var detailId = Guid.Parse("eeeeeeee-1111-2222-3333-ffffffffffff");

            var fetcher = Substitute.For<IFetcher>();
            fetcher.Fetch<List<ServiceCardDto>>(
                    Arg.Any<string>(),
                    "api/services/card-list",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<ServiceCardDto>
                {
                    new()
                    {
                        EntityId = cardId.ToString(),
                        EntityName = "Service",
                        ImagePath = "img.png",
                        BadgeText = "Badge",
                        Address = "Addr"
                    }
                });
            fetcher.Fetch<ServiceCardDetailDto>(
                    Arg.Any<string>(),
                    "api/services/detail/{identifier}",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new ServiceCardDetailDto
                {
                    Identifier = detailId.ToString(),
                    Name = "Service",
                    MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "Milano", LogoPath = "logo" }
                });

            var collector = new ServiceCollector(fetcher, configuration);

            var result = await collector.GetEntities("Milano");

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Detail, Is.Not.Null);
            NUnitAssert.That(result[0].Detail!.Identifier, Is.EqualTo(cardId));
            NUnitAssert.That(result[0].Detail!.Name, Is.EqualTo("Service"));
        }
    }
}
