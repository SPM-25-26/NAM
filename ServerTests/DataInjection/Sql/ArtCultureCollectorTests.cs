using DataInjection.Core.Interfaces;
using DataInjection.SQL.Collectors;
using DataInjection.SQL.DTOs;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class ArtCultureCollectorTests
    {
        private IFetcher _fetcher = null!;
        private IConfiguration _configuration = null!;

        [SetUp]
        public void Setup()
        {
            _fetcher = Substitute.For<IFetcher>();
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "DataInjectionApi", "https://api.example.com" }
                })
                .Build();
        }

        [Test]
        public async Task GetEntities_ReturnsEmpty_WhenCardListEmpty()
        {
            _fetcher.Fetch<List<ArtCultureNatureCardDto>>(
                Arg.Any<string>(),
                "api/art-culture/card-list",
                Arg.Any<Dictionary<string, string?>>(),
                Arg.Any<CancellationToken>())
                .Returns(new List<ArtCultureNatureCardDto>());

            var collector = new ArtCultureCollector(_fetcher, _configuration);

            var result = await collector.GetEntities("Milano");

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetEntities_MapsCardAndDetail_WithNullValues()
        {
            var cardId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            _fetcher.Fetch<List<ArtCultureNatureCardDto>>(
                Arg.Any<string>(),
                "api/art-culture/card-list",
                Arg.Any<Dictionary<string, string?>>(),
                Arg.Any<CancellationToken>())
                .Returns(new List<ArtCultureNatureCardDto>
                {
                    new()
                    {
                        EntityId = cardId.ToString(),
                        EntityName = null!,
                        ImagePath = null!,
                        BadgeText = null!,
                        Address = null!
                    }
                });

            _fetcher.Fetch<ArtCultureNatureDetailDto>(
                Arg.Any<string>(),
                "api/art-culture/detail/{identifier}",
                Arg.Any<Dictionary<string, string?>>(),
                Arg.Any<CancellationToken>())
                .Returns(new ArtCultureNatureDetailDto
                {
                    Identifier = null,
                    OfficialName = null,
                    PrimaryImagePath = null,
                    FullAddress = null,
                    Type = null,
                    SubjectDiscipline = null,
                    Description = null,
                    Email = null,
                    Telephone = null,
                    Website = null,
                    Instagram = null,
                    Facebook = null,
                    MunicipalityData = null!
                });

            var collector = new ArtCultureCollector(_fetcher, _configuration);

            var result = await collector.GetEntities("Milano");

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            var card = result[0];
            NUnitAssert.That(card.EntityId, Is.EqualTo(cardId));
            NUnitAssert.That(card.EntityName, Is.EqualTo(string.Empty));
            NUnitAssert.That(card.Detail, Is.Not.Null);
            NUnitAssert.That(card.Detail!.Identifier, Is.EqualTo(cardId));
            NUnitAssert.That(card.Detail.OfficialName, Is.EqualTo(string.Empty));
        }
    }
}
