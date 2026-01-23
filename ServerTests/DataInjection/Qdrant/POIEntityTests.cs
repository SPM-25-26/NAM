using NUnitAssert = NUnit.Framework.Assert;
using DataInjection.Qdrant.Data;
using NUnit.Framework;

namespace nam.ServerTests.DataInjection.Qdrant
{
    [TestFixture]
    public class POIEntityTests
    {
        [Test]
        public void POIEntity_CanBeInstantiated()
        {
            // Act
            var entity = new POIEntity();

            // Assert
            NUnitAssert.That(entity, Is.Not.Null);
        }

        [Test]
        public void POIEntity_CanSetAndGetProperties()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entityId = "test-123";
            var city = "Milano";
            var apiEndpoint = "/api/test";
            var lat = 45.4642;
            var lon = 9.1900;
            var chunkPart = 1;

            // Act
            var entity = new POIEntity
            {
                Id = id,
                EntityId = entityId,
                city = city,
                apiEndpoint = apiEndpoint,
                lat = lat,
                lon = lon,
                chunkPart = chunkPart
            };

            // Assert
            NUnitAssert.That(entity.Id, Is.EqualTo(id));
            NUnitAssert.That(entity.EntityId, Is.EqualTo(entityId));
            NUnitAssert.That(entity.city, Is.EqualTo(city));
            NUnitAssert.That(entity.apiEndpoint, Is.EqualTo(apiEndpoint));
            NUnitAssert.That(entity.lat, Is.EqualTo(lat));
            NUnitAssert.That(entity.lon, Is.EqualTo(lon));
            NUnitAssert.That(entity.chunkPart, Is.EqualTo(chunkPart));
        }

        [Test]
        public void POIEntity_Vector_CanBeSetAndGet()
        {
            // Arrange
            var vectorData = new float[3072];
            for (int i = 0; i < 3072; i++)
            {
                vectorData[i] = i * 0.001f;
            }
            var vector = new ReadOnlyMemory<float>(vectorData);

            // Act
            var entity = new POIEntity { Vector = vector };

            // Assert
            NUnitAssert.That(entity.Vector.Length, Is.EqualTo(3072));
            NUnitAssert.That(entity.Vector.Span[0], Is.EqualTo(0f));
            NUnitAssert.That(entity.Vector.Span[100], Is.EqualTo(0.1f).Within(0.0001f));
        }

        [Test]
        public void POIEntity_HandlesEmptyStrings()
        {
            // Act
            var entity = new POIEntity
            {
                EntityId = "",
                city = "",
                apiEndpoint = ""
            };

            // Assert
            NUnitAssert.That(entity.EntityId, Is.Empty);
            NUnitAssert.That(entity.city, Is.Empty);
            NUnitAssert.That(entity.apiEndpoint, Is.Empty);
        }

        [Test]
        public void POIEntity_HandlesNegativeCoordinates()
        {
            // Act
            var entity = new POIEntity
            {
                lat = -45.4642,
                lon = -9.1900
            };

            // Assert
            NUnitAssert.That(entity.lat, Is.EqualTo(-45.4642));
            NUnitAssert.That(entity.lon, Is.EqualTo(-9.1900));
        }

        [Test]
        public void POIEntity_HandlesZeroCoordinates()
        {
            // Act
            var entity = new POIEntity
            {
                lat = 0.0,
                lon = 0.0
            };

            // Assert
            NUnitAssert.That(entity.lat, Is.EqualTo(0.0));
            NUnitAssert.That(entity.lon, Is.EqualTo(0.0));
        }

        [Test]
        public void POIEntity_HandlesExtremeCoordinates()
        {
            // Act
            var entity = new POIEntity
            {
                lat = 90.0,  // North Pole
                lon = 180.0  // International Date Line
            };

            // Assert
            NUnitAssert.That(entity.lat, Is.EqualTo(90.0));
            NUnitAssert.That(entity.lon, Is.EqualTo(180.0));
        }

        [Test]
        public void POIEntity_HandlesUnicodeInStrings()
        {
            // Arrange
            var unicodeCity = "Москва 北京 🌍";
            var unicodeEndpoint = "/api/データ";

            // Act
            var entity = new POIEntity
            {
                city = unicodeCity,
                apiEndpoint = unicodeEndpoint,
                EntityId = "entity-ñoño"
            };

            // Assert
            NUnitAssert.That(entity.city, Is.EqualTo(unicodeCity));
            NUnitAssert.That(entity.apiEndpoint, Is.EqualTo(unicodeEndpoint));
            NUnitAssert.That(entity.EntityId, Is.EqualTo("entity-ñoño"));
        }

        [Test]
        public void POIEntity_HandlesVeryLongStrings()
        {
            // Arrange
            var longString = new string('x', 10000);

            // Act
            var entity = new POIEntity
            {
                EntityId = longString,
                city = longString,
                apiEndpoint = longString
            };

            // Assert
            NUnitAssert.That(entity.EntityId, Has.Length.EqualTo(10000));
            NUnitAssert.That(entity.city, Has.Length.EqualTo(10000));
            NUnitAssert.That(entity.apiEndpoint, Has.Length.EqualTo(10000));
        }

        [Test]
        public void POIEntity_ChunkPart_HandlesNegativeValues()
        {
            // Act
            var entity = new POIEntity { chunkPart = -1 };

            // Assert
            NUnitAssert.That(entity.chunkPart, Is.EqualTo(-1));
        }

        [Test]
        public void POIEntity_ChunkPart_HandlesLargeValues()
        {
            // Act
            var entity = new POIEntity { chunkPart = int.MaxValue };

            // Assert
            NUnitAssert.That(entity.chunkPart, Is.EqualTo(int.MaxValue));
        }
    }
}
