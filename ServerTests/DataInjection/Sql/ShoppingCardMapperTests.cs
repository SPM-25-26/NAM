using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class ShoppingCardMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsEmpty_WhenDtoListNullOrEmpty()
        {
            var mapper = new ShoppingCardMapper();

            NUnitAssert.That(mapper.MapToEntity(null!), Is.Empty);
            NUnitAssert.That(mapper.MapToEntity(new List<ShoppingCardDto>()), Is.Empty);
        }

        [Test]
        public void MapToEntity_MapsValuesAndDefaults()
        {
            var mapper = new ShoppingCardMapper();
            var id = Guid.Parse("77777777-7777-7777-7777-777777777777");

            var dtos = new List<ShoppingCardDto>
            {
                new()
                {
                    EntityId = id.ToString(),
                    EntityName = "Shop",
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
            NUnitAssert.That(result[1].BadgeText, Is.EqualTo(string.Empty));
        }
    }
}
