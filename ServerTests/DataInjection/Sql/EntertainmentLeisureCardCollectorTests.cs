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
    public class EntertainmentLeisureCardCollectorTests
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
            fetcher.Fetch<List<EntertainmentLeisureCardDto>>(
                    Arg.Any<string>(),
                    "api/entertainment-leisure/card-list",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<EntertainmentLeisureCardDto>());

            var collector = new EntertainmentLeisureCardCollector(fetcher, configuration);

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

            var cardId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var detailId = Guid.Parse("66666666-6666-6666-6666-666666666666");

            var fetcher = Substitute.For<IFetcher>();
            fetcher.Fetch<List<EntertainmentLeisureCardDto>>(
                    Arg.Any<string>(),
                    "api/entertainment-leisure/card-list",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<EntertainmentLeisureCardDto>
                {
                    new()
                    {
                        EntityId = cardId.ToString(),
                        EntityName = "Entertainment",
                        ImagePath = "img.png",
                        BadgeText = "Badge",
                        Address = "Addr"
                    }
                });
            fetcher.Fetch<EntertainmentLeisureDetailDto>(
                    Arg.Any<string>(),
                    "api/entertainment-leisure/detail/{identifier}",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new EntertainmentLeisureDetailDto
                {
                    Identifier = detailId.ToString(),
                    OfficialName = "Cinema",
                    Address = "Addr",
                    Category = "Category",
                    PrimaryImage = "img.png",
                    VirtualTours = new List<string> { "tour" },
                    Latitude = 1.2,
                    Longitude = 3.4,
                    AssociatedServices = new List<AssociatedServiceDto>(),
                    Neighbors = new List<FeatureCardDto>(),
                    MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "Milano", LogoPath = "logo" }
                });

            var collector = new EntertainmentLeisureCardCollector(fetcher, configuration);

            var result = await collector.GetEntities("Milano");

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Detail, Is.Not.Null);
            NUnitAssert.That(result[0].Detail!.Identifier, Is.EqualTo(cardId));
            NUnitAssert.That(result[0].Detail!.OfficialName, Is.EqualTo("Cinema"));
        }
    }
}
