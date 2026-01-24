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
    public class ShoppingCollectorTests
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
            fetcher.Fetch<List<ShoppingCardDto>>(
                    Arg.Any<string>(),
                    "api/shopping/card-list",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<ShoppingCardDto>());

            var collector = new ShoppingCollector(fetcher, configuration);

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

            var cardId = Guid.Parse("11111111-2222-3333-4444-555555555555");
            var detailId = Guid.Parse("66666666-7777-8888-9999-000000000000");

            var fetcher = Substitute.For<IFetcher>();
            fetcher.Fetch<List<ShoppingCardDto>>(
                    Arg.Any<string>(),
                    "api/shopping/card-list",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<ShoppingCardDto>
                {
                    new()
                    {
                        EntityId = cardId.ToString(),
                        EntityName = "Shop",
                        ImagePath = "img.png",
                        BadgeText = "Badge",
                        Address = "Addr"
                    }
                });
            fetcher.Fetch<ShoppingCardDetailDto>(
                    Arg.Any<string>(),
                    "api/shopping/detail/{identifier}",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new ShoppingCardDetailDto
                {
                    Identifier = detailId.ToString(),
                    OfficialName = "Shop",
                    MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "Milano", LogoPath = "logo" }
                });

            var collector = new ShoppingCollector(fetcher, configuration);

            var result = await collector.GetEntities("Milano");

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Detail, Is.Not.Null);
            NUnitAssert.That(result[0].Detail!.Identifier, Is.EqualTo(cardId));
            NUnitAssert.That(result[0].Detail!.OfficialName, Is.EqualTo("Shop"));
        }
    }
}
