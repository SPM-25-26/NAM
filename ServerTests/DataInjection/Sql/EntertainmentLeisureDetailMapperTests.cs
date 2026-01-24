using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class EntertainmentLeisureDetailMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsNewIdentifier_WhenDtoNull()
        {
            var mapper = new EntertainmentLeisureDetailMapper();

            var result = mapper.MapToEntity(null!);

            NUnitAssert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void MapToEntity_MapsCollectionsAndNestedData()
        {
            var mapper = new EntertainmentLeisureDetailMapper();
            var identifier = Guid.Parse("22222222-2222-2222-2222-222222222222");

            var dto = new EntertainmentLeisureDetailDto
            {
                Identifier = identifier.ToString(),
                OfficialName = "Name",
                Address = "Address",
                Category = "Cat",
                PrimaryImage = "img.png",
                Description = "Desc",
                Latitude = 1.2,
                Longitude = 3.4,
                Gallery = new List<string> { "img1", null! },
                VirtualTours = new List<string> { "tour" },
                Neighbors = new List<FeatureCardDto>
                {
                    new() { EntityId = Guid.Empty.ToString(), Title = "Neighbor", Category = MobileCategory.Services }
                },
                NearestCarPark = new NearestCarParkDto { Latitude = 1, Longitude = 2, Address = "Park", Distance = 3 },
                AssociatedServices = new List<AssociatedServiceDto> { new() { Identifier = Guid.Empty.ToString(), Name = "Assoc" } },
                MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "City", LogoPath = "logo" }
            };

            var result = mapper.MapToEntity(dto);

            NUnitAssert.That(result.Identifier, Is.EqualTo(identifier));
            NUnitAssert.That(result.Gallery, Has.Count.EqualTo(2));
            NUnitAssert.That(result.VirtualTours, Has.Count.EqualTo(1));
            NUnitAssert.That(result.Neighbors, Has.Count.EqualTo(1));
            NUnitAssert.That(result.AssociatedServices, Has.Count.EqualTo(1));
            NUnitAssert.That(result.AssociatedServices.First().Identifier, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result.MunicipalityData, Is.Not.Null);
        }
    }
}
