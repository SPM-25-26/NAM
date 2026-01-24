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
    public class ArtCultureCollectorTests
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
            fetcher.Fetch<List<ArtCultureNatureCardDto>>(
                    Arg.Any<string>(),
                    "api/art-culture/card-list",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<ArtCultureNatureCardDto>());

            var collector = new ArtCultureCollector(fetcher, configuration);

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

            var cardId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var detailId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

            var fetcher = Substitute.For<IFetcher>();
            fetcher.Fetch<List<ArtCultureNatureCardDto>>(
                    Arg.Any<string>(),
                    "api/art-culture/card-list",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<ArtCultureNatureCardDto>
                {
                    new()
                    {
                        EntityId = cardId.ToString(),
                        EntityName = "Art",
                        ImagePath = "img.png",
                        BadgeText = "Badge",
                        Address = "Addr"
                    }
                });
            fetcher.Fetch<ArtCultureNatureDetailDto>(
                    Arg.Any<string>(),
                    "api/art-culture/detail/{identifier}",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new ArtCultureNatureDetailDto
                {
                    Identifier = detailId.ToString(),
                    OfficialName = "Museum",
                    MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "Milano", LogoPath = "logo" }
                });

            var collector = new ArtCultureCollector(fetcher, configuration);

            var result = await collector.GetEntities("Milano");

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Detail, Is.Not.Null);
            NUnitAssert.That(result[0].Detail!.Identifier, Is.EqualTo(cardId));
            NUnitAssert.That(result[0].Detail!.OfficialName, Is.EqualTo("Museum"));
        }
    }
}
