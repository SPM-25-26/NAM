using DataInjection.Qdrant.Mappers;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;
using DataInjection.Core.Interfaces;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Qdrant
{
    [TestFixture]
    public class QdrantMappersTests
    {
        [Test]
        public void ArticleQdrantMapper_MapsApiEndpointAndCity()
        {
            var mapper = new ArticleQdrantMapper();
            var card = new ArticleCard
            {
                EntityId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                EntityName = "Article",
                BadgeText = "Badge",
                ImagePath = "img.png",
                Address = "Address",
                Detail = new ArticleDetail
                {
                    Identifier = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Title = "Title",
                    Script = "Script",
                    ImagePath = "detail.png",
                    UpdatedAt = new DateTime(2024, 1, 1),
                    MunicipalityData = new MunicipalityForLocalStorageSetting { Name = "Milano" }
                }
            };

            var result = mapper.MapToEntity(card);

            NUnitAssert.That(result.apiEndpoint, Is.EqualTo("/api/article/card"));
            NUnitAssert.That(result.EntityId, Is.EqualTo(card.EntityId.ToString()));
            NUnitAssert.That(result.city, Is.EqualTo("Milano"));
        }

        [Test]
        public void OrganizationQdrantMapper_MapsApiEndpointAndCoordinates()
        {
            var mapper = new OrganizationQdrantMapper();
            var card = new OrganizationCard
            {
                TaxCode = "ORG123",
                EntityName = "Org",
                ImagePath = "img.png",
                BadgeText = "Badge",
                Address = "Address",
                Detail = new OrganizationMobileDetail
                {
                    TaxCode = "ORG123",
                    LegalName = "Org Legal",
                    PrimaryImagePath = "detail.png",
                    Type = "Type",
                    Address = "Address",
                    Description = "Desc",
                    MainFunction = "Main",
                    Latitude = 45.0,
                    Longitude = 9.0,
                    MunicipalityData = new MunicipalityForLocalStorageSetting { Name = "Roma" }
                }
            };

            var result = mapper.MapToEntity(card);

            NUnitAssert.That(result.apiEndpoint, Is.EqualTo("/api/organizations/card"));
            NUnitAssert.That(result.EntityId, Is.EqualTo("ORG123"));
            NUnitAssert.That(result.city, Is.EqualTo("Roma"));
            NUnitAssert.That(result.lat, Is.EqualTo(45.0));
            NUnitAssert.That(result.lon, Is.EqualTo(9.0));
        }

        [Test]
        public void ArtCultureQdrantMapper_MapsApiEndpointAndCoordinates()
        {
            var mapper = new ArtCultureQdrantMapper();
            var card = new ArtCultureNatureCard
            {
                EntityId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                EntityName = "Art",
                ImagePath = "img.png",
                BadgeText = "Badge",
                Address = "Address",
                Detail = new ArtCultureNatureDetail
                {
                    Identifier = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    OfficialName = "Official",
                    PrimaryImagePath = "detail.png",
                    FullAddress = "Address",
                    Type = "Type",
                    SubjectDiscipline = "Subject",
                    Description = "Desc",
                    Latitude = 41.9,
                    Longitude = 12.5,
                    MunicipalityData = new MunicipalityForLocalStorageSetting { Name = "Napoli" }
                }
            };

            var result = ((IDtoMapper<ArtCultureNatureCard, POIEntity>)mapper).MapToEntity(card);

            NUnitAssert.That(result.apiEndpoint, Is.EqualTo("/api/art-culture/card"));
            NUnitAssert.That(result.EntityId, Is.EqualTo(card.EntityId.ToString()));
            NUnitAssert.That(result.city, Is.EqualTo("Napoli"));
            NUnitAssert.That(result.lat, Is.EqualTo(41.9));
            NUnitAssert.That(result.lon, Is.EqualTo(12.5));
        }

        [Test]
        public void PublicEventQdrantMapper_MapsApiEndpointAndCoordinates()
        {
            var mapper = new PublicEventQdrantMapper();
            var card = new PublicEventCard
            {
                EntityId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                EntityName = "Event",
                ImagePath = "img.png",
                BadgeText = "Badge",
                Address = "Address",
                MunicipalityData = new MunicipalityForLocalStorageSetting { Name = "Genova" },
                Date = "2024-01-01",
                Detail = new PublicEventMobileDetail
                {
                    Identifier = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Title = "Title",
                    Latitude = 44.4,
                    Longitude = 8.9,
                    MunicipalityData = new MunicipalityForLocalStorageSetting { Name = "Genova" }
                }
            };

            var result = mapper.MapToEntity(card);

            NUnitAssert.That(result.apiEndpoint, Is.EqualTo("/api/public-event/card"));
            NUnitAssert.That(result.EntityId, Is.EqualTo(card.EntityId.ToString()));
            NUnitAssert.That(result.city, Is.EqualTo("Genova"));
            NUnitAssert.That(result.lat, Is.EqualTo(44.4));
            NUnitAssert.That(result.lon, Is.EqualTo(8.9));
        }

        [Test]
        public void NatureQdrantMapper_MapsApiEndpointAndCoordinates()
        {
            var mapper = new NatureQdrantMapper();
            var card = new Nature
            {
                EntityId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                EntityName = "Nature",
                ImagePath = "img.png",
                BadgeText = "Badge",
                Address = "Address",
                Detail = new ArtCultureNatureDetail
                {
                    Identifier = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    OfficialName = "Official",
                    PrimaryImagePath = "detail.png",
                    FullAddress = "Address",
                    Type = "Type",
                    SubjectDiscipline = "Subject",
                    Description = "Desc",
                    Latitude = 40.8,
                    Longitude = 14.2,
                    MunicipalityData = new MunicipalityForLocalStorageSetting { Name = "Torino" }
                }
            };

            var result = mapper.MapToEntity(card);

            NUnitAssert.That(result.apiEndpoint, Is.EqualTo("/api/nature/card"));
            NUnitAssert.That(result.EntityId, Is.EqualTo(card.EntityId.ToString()));
            NUnitAssert.That(result.city, Is.EqualTo("Torino"));
            NUnitAssert.That(result.lat, Is.EqualTo(40.8));
            NUnitAssert.That(result.lon, Is.EqualTo(14.2));
        }
    }
}
