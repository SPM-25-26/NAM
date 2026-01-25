using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class SleepCardDetailMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsNewIdentifier_WhenDtoNull()
        {
            var mapper = new SleepCardDetailMapper();

            var result = mapper.MapToEntity(null!);

            NUnitAssert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void MapToEntity_MapsCollectionsAndTrims()
        {
            var mapper = new SleepCardDetailMapper();
            var identifier = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");

            var dto = new SleepCardDetailDto
            {
                Identifier = identifier.ToString(),
                OfficialName = " Name ",
                Description = " Desc ",
                Classification = " Class ",
                Typology = " Type ",
                PrimaryImage = " img ",
                Email = " mail ",
                Telephone = " tel ",
                Website = " web ",
                Facebook = " fb ",
                Instagram = " insta ",
                Latitude = 1.2,
                Longitude = 3.4,
                Gallery = new List<string?> { " g1 ", " " },
                VirtualTours = new List<string?> { " v1 " },
                Services = new List<string?> { " s1 " },
                RoomTypologies = new List<string?> { " r1 " },
                NearestCarPark = new NearestCarParkDto { Latitude = 1, Longitude = 2, Address = " Park ", Distance = 3 },
                Owner = new OwnerDto { TaxCode = " TAX ", LegalName = " Own ", WebSite = " site " },
                OpeningHours = new OpeningHoursSpecificationDto { Description = " Hours " },
                TemporaryClosure = new TemporaryClosureDto { ReasonForClosure = " Reason ", Description = " Desc " },
                Booking = new BookingDto { Name = BookingType.Mandatory, Description = " Book " },
                ShortAddress = " Short ",
                Offers = new List<OfferDto?> { new() { Description = " Offer " } },
                AssociatedServices = new List<AssociatedServiceDto?> { new() { Identifier = Guid.Empty.ToString(), Name = "Assoc" } },
                Neighbors = new List<FeatureCardDto?> { new() { EntityId = Guid.Empty.ToString(), Title = "Neighbor", Category = MobileCategory.Services } },
                MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "City", LogoPath = "logo" }
            };

            var result = mapper.MapToEntity(dto);

            NUnitAssert.That(result.Identifier, Is.EqualTo(identifier));
            NUnitAssert.That(result.OfficialName, Is.EqualTo("Name"));
            NUnitAssert.That(result.Gallery, Has.Count.EqualTo(1));
            NUnitAssert.That(result.VirtualTours, Has.Count.EqualTo(1));
            NUnitAssert.That(result.Services, Has.Count.EqualTo(1));
            NUnitAssert.That(result.RoomTypologies, Has.Count.EqualTo(1));
            NUnitAssert.That(result.AssociatedServices, Has.Count.EqualTo(1));
            NUnitAssert.That(result.AssociatedServices.First().Identifier, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result.Neighbors, Has.Count.EqualTo(1));
        }
    }
}
