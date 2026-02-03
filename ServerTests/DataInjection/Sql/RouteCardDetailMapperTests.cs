using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class RouteCardDetailMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsNewIdentifier_WhenDtoNull()
        {
            var mapper = new RouteCardDetailMapper();

            var result = mapper.MapToEntity(null!);

            NUnitAssert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public void MapToEntity_MapsCollectionsAndTrims()
        {
            var mapper = new RouteCardDetailMapper();

            var dto = new RouteDetailDto
            {
                ImagePath = " img.png ",
                Name = " Name ",
                Gallery = new List<string?> { " g1 ", " " },
                VirtualTours = new List<string?> { " v1 " },
                BestWhen = new List<string?> { " bw " },
                StartingPoint = new PointDto { Address = " Addr ", Latitude = 1, Longitude = 2 },
                MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "City", LogoPath = "logo" },
                Stages = new List<StageMobileDto?>
                {
                    new()
                    {
                        PoiIdentifier = Guid.Empty.ToString(),
                        PoiOfficialName = "Poi",
                        Category = "Cat"
                    }
                },
                StagesPoi = new List<FeatureCardDto?>
                {
                    new() { EntityId = Guid.Empty.ToString(), Title = "Stage", Category = MobileCategory.Services }
                }
            };

            var result = mapper.MapToEntity(dto);

            NUnitAssert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result.Gallery, Has.Count.EqualTo(1));
            NUnitAssert.That(result.VirtualTours, Has.Count.EqualTo(1));
            NUnitAssert.That(result.BestWhen, Has.Count.EqualTo(1));
            NUnitAssert.That(result.StartingPoint, Is.Not.Null);
            NUnitAssert.That(result.MunicipalityData, Is.Not.Null);
            NUnitAssert.That(result.Stages, Has.Count.EqualTo(1));
            NUnitAssert.That(result.Stages.First().StageMobile!.PoiIdentifier, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result.StagesPoi, Has.Count.EqualTo(1));
        }
    }
}
