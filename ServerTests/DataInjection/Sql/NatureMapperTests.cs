using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class NatureMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsEmpty_WhenListEmpty()
        {
            var mapper = new NatureMapper();

            var result = mapper.MapToEntity(new List<ArtCultureNatureCardDto>());

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public void MapToEntity_MapsValuesAndSkipsNulls()
        {
            var mapper = new NatureMapper();
            var id = Guid.Parse("33333333-3333-3333-3333-333333333333");

            var dtos = new List<ArtCultureNatureCardDto?>
            {
                null,
                new()
                {
                    EntityId = id.ToString(),
                    EntityName = "Nature",
                    ImagePath = "img.png",
                    BadgeText = "Badge",
                    Address = "Addr"
                },
                new()
                {
                    EntityId = "invalid",
                    EntityName = null!,
                    ImagePath = null!,
                    BadgeText = null!,
                    Address = null!
                }
            };

            var result = mapper.MapToEntity(dtos!);

            NUnitAssert.That(result, Has.Count.EqualTo(2));
            NUnitAssert.That(result[0].EntityId, Is.EqualTo(id));
            NUnitAssert.That(result[1].EntityId, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result[1].EntityName, Is.EqualTo(string.Empty));
        }
    }
}
