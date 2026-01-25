using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class SleepCardMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsEmpty_WhenDtoListNullOrEmpty()
        {
            var mapper = new SleepCardMapper();

            NUnitAssert.That(mapper.MapToEntity(null!), Is.Empty);
            NUnitAssert.That(mapper.MapToEntity(new List<SleepCardDto>()), Is.Empty);
        }

        [Test]
        public void MapToEntity_MapsValuesAndDefaults()
        {
            var mapper = new SleepCardMapper();
            var id = Guid.Parse("99999999-9999-9999-9999-999999999999");

            var dtos = new List<SleepCardDto>
            {
                new()
                {
                    EntityId = id.ToString(),
                    EntityName = "Sleep",
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

            var result = mapper.MapToEntity(dtos);

            NUnitAssert.That(result, Has.Count.EqualTo(2));
            NUnitAssert.That(result[0].EntityId, Is.EqualTo(id));
            NUnitAssert.That(result[1].EntityId, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result[1].EntityName, Is.EqualTo(string.Empty));
        }
    }
}
