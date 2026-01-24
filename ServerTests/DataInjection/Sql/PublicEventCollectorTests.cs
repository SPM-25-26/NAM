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
    public class PublicEventCollectorTests
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
            fetcher.Fetch<List<PublicEventCardDto>>(
                    Arg.Any<string>(),
                    "api/events/card-list",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<PublicEventCardDto>());

            var collector = new PublicEventCollector(fetcher, configuration);

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

            var cardId = Guid.Parse("99999999-9999-9999-9999-999999999999");
            var detailId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

            var fetcher = Substitute.For<IFetcher>();
            fetcher.Fetch<List<PublicEventCardDto>>(
                    Arg.Any<string>(),
                    "api/events/card-list",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<PublicEventCardDto>
                {
                    new()
                    {
                        EntityId = cardId.ToString(),
                        EntityName = "Event",
                        ImagePath = "img.png",
                        BadgeText = "Badge",
                        Address = "Addr",
                        Date = "2024-01-01"
                    }
                });
            fetcher.Fetch<PublicEventMobileDetailDto>(
                    Arg.Any<string>(),
                    "api/events/detail/{identifier}",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new PublicEventMobileDetailDto
                {
                    Identifier = detailId.ToString(),
                    Title = "Event",
                    MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "Milano", LogoPath = "logo" }
                });

            var collector = new PublicEventCollector(fetcher, configuration);

            var result = await collector.GetEntities("Milano");

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Detail, Is.Not.Null);
            NUnitAssert.That(result[0].Detail!.Identifier, Is.EqualTo(cardId));
            NUnitAssert.That(result[0].Detail!.Title, Is.EqualTo("Event"));
        }
    }
}
