using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class ArtCultureCardMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsEmpty_WhenDtoListNullOrEmpty()
        {
            var mapper = new ArtCultureCardMapper();

            NUnitAssert.That(mapper.MapToEntity(null!), Is.Empty);
            NUnitAssert.That(mapper.MapToEntity(new List<ArtCultureNatureCardDto>()), Is.Empty);
        }

        [Test]
        public void MapToEntity_MapsValuesAndDefaults()
        {
            var mapper = new ArtCultureCardMapper();
            var id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            var dtos = new List<ArtCultureNatureCardDto>
            {
                new()
                {
                    EntityId = id.ToString(),
                    EntityName = "Art",
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
            NUnitAssert.That(result[0].EntityName, Is.EqualTo("Art"));
            NUnitAssert.That(result[1].EntityId, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result[1].EntityName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void MapToEntity_SkipsNullDtos()
        {
            var mapper = new ArtCultureCardMapper();

            var result = mapper.MapToEntity(new List<ArtCultureNatureCardDto?> { null!, new ArtCultureNatureCardDto { EntityId = Guid.Empty.ToString() } }!);

            NUnitAssert.That(result, Has.Count.EqualTo(1));
        }
    }
}
