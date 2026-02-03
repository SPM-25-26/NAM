using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class EatAndDrinkDetailMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsNewIdentifier_WhenDtoNull()
        {
            var mapper = new EatAndDrinkDetailMapper();

            var result = mapper.MapToEntity(null!);

            NUnitAssert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void MapToEntity_MapsCollectionsAndTrimsValues()
        {
            var mapper = new EatAndDrinkDetailMapper();
            var identifier = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");

            var dto = new EatAndDrinkDetailDto
            {
                Identifier = identifier.ToString(),
                PrimaryImagePath = " img.png ",
                OfficialName = " Name ",
                Address = " Address ",
                Description = " Desc ",
                Email = " mail ",
                Telephone = " phone ",
                Facebook = " fb ",
                Instagram = " insta ",
                Website = " web ",
                Type = " Type ",
                Latitude = 1.2,
                Longitude = 3.4,
                Gallery = new List<string?> { " img1 ", " " },
                VirtualTours = new List<string?> { " tour " },
                DietaryNeeds = new List<string?> { " veg ", null },
                Services = new List<OntoremaServiceDto?> { new() { Name = " S ", Description = " D " } },
                TypicalProducts = new List<TypicalProductMobileDto?> { new() { Name = " P ", Description = " PD " } },
                Owner = new OwnerDto { TaxCode = " TAX ", LegalName = " Own ", WebSite = " site " },
                NearestCarPark = new NearestCarParkDto { Latitude = 1, Longitude = 2, Address = " Park ", Distance = 3 },
                OpeningHours = new OpeningHoursSpecificationDto
                {
                    Description = " Hours ",
                    AdmissionType = new AdmissionTypeDto { Name = AdmissionTypeName.Daily, Description = " Adm " }
                },
                TemporaryClosure = new TemporaryClosureDto { ReasonForClosure = " Reason ", Description = " Desc " },
                Booking = new BookingDto { Name = BookingType.Mandatory, Description = " Book " },
                MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "City", LogoPath = "logo" },
                Neighbors = new List<FeatureCardDto?> { new() { EntityId = Guid.Empty.ToString(), Title = "Neighbor", Category = MobileCategory.Services } },
                AssociatedServices = new List<AssociatedServiceDto?> { new() { Identifier = Guid.Empty.ToString(), Name = "Assoc", ImagePath = "img" } }
            };

            var result = mapper.MapToEntity(dto);

            NUnitAssert.That(result.Identifier, Is.EqualTo(identifier));
            NUnitAssert.That(result.OfficialName, Is.EqualTo("Name"));
            NUnitAssert.That(result.Gallery, Has.Count.EqualTo(1));
            NUnitAssert.That(result.VirtualTours, Has.Count.EqualTo(1));
            NUnitAssert.That(result.DietaryNeeds, Has.Count.EqualTo(1));
            NUnitAssert.That(result.Services, Has.Count.EqualTo(1));
            NUnitAssert.That(result.TypicalProducts, Has.Count.EqualTo(1));
            NUnitAssert.That(result.Owner, Is.Not.Null);
            NUnitAssert.That(result.Neighbors, Has.Count.EqualTo(1));
            NUnitAssert.That(result.AssociatedServices.First().Identifier, Is.Not.EqualTo(Guid.Empty));
        }
    }
}
