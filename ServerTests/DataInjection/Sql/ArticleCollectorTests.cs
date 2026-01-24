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
    public class ArticleCollectorTests
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
            fetcher.Fetch<List<ArticleCardDto>>(
                    Arg.Any<string>(),
                    "api/articles/card-list",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<ArticleCardDto>());

            var collector = new ArticleCollector(fetcher, configuration);

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

            var cardId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var detailId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            var fetcher = Substitute.For<IFetcher>();
            fetcher.Fetch<List<ArticleCardDto>>(
                    Arg.Any<string>(),
                    "api/articles/card-list",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<ArticleCardDto>
                {
                    new()
                    {
                        entityId = cardId.ToString(),
                        EntityName = "Article",
                        BadgeText = "Badge",
                        ImagePath = "img.png",
                        Address = "Addr"
                    }
                });
            fetcher.Fetch<ArticleDetailDto>(
                    Arg.Any<string>(),
                    "api/articles/detail/{identifier}",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new ArticleDetailDto
                {
                    Identifier = detailId.ToString(),
                    Title = "Title",
                    Script = "Script",
                    ImagePath = "detail.png",
                    UpdatedAt = new DateTime(2024, 1, 1),
                    Themes = new List<string> { "Theme" },
                    Paragraphs = new List<ParagraphDto>(),
                    MunicipalityData = new MunicipalityForLocalStorageSetting { Name = "Milano", LogoPath = "logo" }
                });

            var collector = new ArticleCollector(fetcher, configuration);

            var result = await collector.GetEntities("Milano");

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Detail, Is.Not.Null);
            NUnitAssert.That(result[0].Detail!.Identifier, Is.EqualTo(cardId));
            NUnitAssert.That(result[0].Detail!.Title, Is.EqualTo("Title"));
        }
    }
}
