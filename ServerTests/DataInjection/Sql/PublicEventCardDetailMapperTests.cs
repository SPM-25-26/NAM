using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class PublicEventCardDetailMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsNewIdentifier_WhenDtoNull()
        {
            var mapper = new PublicEventCardDetailMapper();

            var result = mapper.MapToEntity(null!);

            NUnitAssert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void MapToEntity_MapsNestedDataAndCollections()
        {
            var mapper = new PublicEventCardDetailMapper();
            var identifier = Guid.Parse("55555555-5555-5555-5555-555555555555");

            var dto = new PublicEventMobileDetailDto
            {
                Identifier = identifier.ToString(),
                Title = " Title ",
                Address = " Address ",
                Description = " Desc ",
                Typology = " Type ",
                PrimaryImage = " img.png ",
                Gallery = new List<string> { "g1" },
                VirtualTours = new List<string> { "v1" },
                Audience = " All ",
                Email = " mail ",
                Telephone = " tel ",
                Website = " web ",
                Facebook = " fb ",
                Instagram = " insta ",
                Latitude = 1.2,
                Longitude = 3.4,
                Date = new DateTime(2024, 1, 1),
                Organizer = new OrganizerDto { TaxCode = " TAX ", LegalName = " Org ", Website = " web " },
                TicketsAndCosts = new List<OfferDto> { new() { Description = " Offer " } },
                MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "City", LogoPath = "logo" },
                Neighbors = new List<FeatureCardDto>
                {
                    new() { EntityId = Guid.Empty.ToString(), Title = "Neighbor", Category = MobileCategory.Services }
                },
                NearestCarPark = new NearestCarParkDto { Latitude = 1, Longitude = 2, Address = " Park ", Distance = 3 }
            };

            var result = mapper.MapToEntity(dto);

            NUnitAssert.That(result.Identifier, Is.EqualTo(identifier));
            NUnitAssert.That(result.Title, Is.EqualTo("Title"));
            NUnitAssert.That(result.NearestCarPark, Is.Not.Null);
            NUnitAssert.That(result.Organizer, Is.Not.Null);
            NUnitAssert.That(result.TicketsAndCosts, Has.Count.EqualTo(1));
            NUnitAssert.That(result.MunicipalityData, Is.Not.Null);
            NUnitAssert.That(result.Neighbors, Has.Count.EqualTo(1));
        }
    }
}
