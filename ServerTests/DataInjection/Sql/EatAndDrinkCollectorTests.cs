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
    public class EatAndDrinkCollectorTests
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
            fetcher.Fetch<List<EatAndDrinkCardDto>>(
                    Arg.Any<string>(),
                    "api/eat-and-drink/card-list",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<EatAndDrinkCardDto>());

            var collector = new EatAndDrinkCollector(fetcher, configuration);

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

            var cardId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var detailId = Guid.Parse("44444444-4444-4444-4444-444444444444");

            var fetcher = Substitute.For<IFetcher>();
            fetcher.Fetch<List<EatAndDrinkCardDto>>(
                    Arg.Any<string>(),
                    "api/eat-and-drink/card-list",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<EatAndDrinkCardDto>
                {
                    new()
                    {
                        EntityId = cardId.ToString(),
                        EntityName = "Eat",
                        ImagePath = "img.png",
                        BadgeText = "Badge",
                        Address = "Addr"
                    }
                });
            fetcher.Fetch<EatAndDrinkDetailDto>(
                    Arg.Any<string>(),
                    "api/eat-and-drink/detail/{identifier}",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new EatAndDrinkDetailDto
                {
                    Identifier = detailId.ToString(),
                    OfficialName = "Restaurant",
                    MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "Milano", LogoPath = "logo" }
                });

            var collector = new EatAndDrinkCollector(fetcher, configuration);

            var result = await collector.GetEntities("Milano");

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Detail, Is.Not.Null);
            NUnitAssert.That(result[0].Detail!.Identifier, Is.EqualTo(cardId));
            NUnitAssert.That(result[0].Detail!.OfficialName, Is.EqualTo("Restaurant"));
        }
    }
}
