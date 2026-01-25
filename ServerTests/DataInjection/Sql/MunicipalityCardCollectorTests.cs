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
    public class MunicipalityCardCollectorTests
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
            fetcher.Fetch<List<MunicipalityCardDto>>(
                    Arg.Any<string>(),
                    "api/organizations/municipalities",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<MunicipalityCardDto>());

            var collector = new MunicipalityCardCollector(fetcher, configuration);

            var result = await collector.GetEntities("Milano");

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetEntities_LinksDetailWithLegalName()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "DataInjectionApi", "https://api.example.com" }
                })
                .Build();

            var fetcher = Substitute.For<IFetcher>();
            fetcher.Fetch<List<MunicipalityCardDto>>(
                    Arg.Any<string>(),
                    "api/organizations/municipalities",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new List<MunicipalityCardDto>
                {
                    new()
                    {
                        LegalName = "Milano",
                        ImagePath = "img.png"
                    }
                });
            fetcher.Fetch<MunicipalityHomeInfoDto>(
                    Arg.Any<string>(),
                    "api/organizations/municipalities/visit",
                    Arg.Any<Dictionary<string, string?>>(),
                    Arg.Any<CancellationToken>())
                .Returns(new MunicipalityHomeInfoDto
                {
                    Name = "Milano",
                    LegalName = "Milano",
                    Contacts = new MunicipalityHomeContactInfoDto()
                });

            var collector = new MunicipalityCardCollector(fetcher, configuration);

            var result = await collector.GetEntities("Milano");

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Detail, Is.Not.Null);
            NUnitAssert.That(result[0].Detail!.LegalName, Is.EqualTo("Milano"));
        }
    }
}
