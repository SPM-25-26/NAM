using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class OrganizationMobileDetailMapperTests
    {
        [Test]
        public void MapToEntity_MapsNestedCollections()
        {
            var mapper = new OrganizationMobileDetailMapper();

            var dto = new OrganizationMobileDetailDto
            {
                TaxCode = "TAX",
                LegalName = "Name",
                PrimaryImagePath = "img.png",
                Latitude = 1.2,
                Longitude = 3.4,
                NearestCarPark = new NearestCarParkDto { Latitude = 1, Longitude = 2, Address = "Park", Distance = 3 },
                OwnedPoi = new List<OwnedPoiDto>
                {
                    new() { Identifier = Guid.Empty.ToString(), OfficialName = "Poi", ImagePath = "poi.png", Category = "Cat" }
                },
                Offers = new List<OfferDto>
                {
                    new() { Description = "Offer", Currency = Currency.EUR }
                },
                Events = new List<PublicEventCardDto>
                {
                    new() { EntityId = Guid.Empty.ToString(), EntityName = "Event", MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "City" } }
                },
                MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "City", LogoPath = "logo" },
                Neighbors = new List<FeatureCardDto>
                {
                    new() { EntityId = Guid.Empty.ToString(), Title = "Neighbor", Category = MobileCategory.Services }
                }
            };

            var result = mapper.MapToEntity(dto);

            NUnitAssert.That(result.TaxCode, Is.EqualTo("TAX"));
            NUnitAssert.That(result.NearestCarPark, Is.Not.Null);
            NUnitAssert.That(result.OwnedPoi, Has.Count.EqualTo(1));
            NUnitAssert.That(result.OwnedPoi!.First().Id, Is.EqualTo(Guid.Empty));
            NUnitAssert.That(result.Offers, Has.Count.EqualTo(1));
            NUnitAssert.That(result.Events, Has.Count.EqualTo(1));
            NUnitAssert.That(result.Events!.First().EntityName, Is.EqualTo("Event"));
            NUnitAssert.That(result.MunicipalityData, Is.Not.Null);
            NUnitAssert.That(result.Neighbors, Has.Count.EqualTo(1));
        }
    }
}
