using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class MapDataMapperTests
    {
        [Test]
        public void MapToEntity_MapsDefaults_WhenMarkersNull()
        {
            var mapper = new MapDataMapper();
            var dto = new MapDataDto
            {
                CenterLatitude = null,
                CenterLongitude = null,
                Markers = null
            };

            var result = mapper.MapToEntity(dto);

            NUnitAssert.That(result.CenterLatitude, Is.EqualTo(0));
            NUnitAssert.That(result.CenterLongitude, Is.EqualTo(0));
            NUnitAssert.That(result.Marker, Is.Empty);
        }

        [Test]
        public void MapToEntity_MapsMarkersAndDefaults()
        {
            var mapper = new MapDataMapper();
            var markerId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

            var dto = new MapDataDto
            {
                CenterLatitude = 45.1,
                CenterLongitude = 9.2,
                Markers = new List<MapMarkerDto>
                {
                    new()
                    {
                        Id = markerId.ToString(),
                        ImagePath = "marker.png",
                        Name = "Marker",
                        Typology = "Type",
                        Address = "Somewhere",
                        Latitude = 45.0,
                        Longitude = 9.0
                    },
                    new()
                    {
                        Id = "invalid",
                        ImagePath = null,
                        Name = null,
                        Typology = null,
                        Address = null,
                        Latitude = null,
                        Longitude = null
                    },
                    null!
                }!
            };

            var result = mapper.MapToEntity(dto);

            NUnitAssert.That(result.Marker, Has.Count.EqualTo(2));
            NUnitAssert.That(result.Marker.First().Id, Is.EqualTo(markerId));
            NUnitAssert.That(result.Marker.First().Name, Is.EqualTo("Marker"));
            NUnitAssert.That(result.Marker.Last().Id, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result.Marker.Last().ImagePath, Is.EqualTo(string.Empty));
            NUnitAssert.That(result.Marker.Last().Address, Is.EqualTo(string.Empty));
            NUnitAssert.That(result.CenterLatitude, Is.EqualTo(45.1));
            NUnitAssert.That(result.CenterLongitude, Is.EqualTo(9.2));
        }
    }
}
