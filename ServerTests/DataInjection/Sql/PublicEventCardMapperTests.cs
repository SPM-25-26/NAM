using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class PublicEventCardMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsEmpty_WhenDtoListNullOrEmpty()
        {
            var mapper = new PublicEventCardMapper();

            NUnitAssert.That(mapper.MapToEntity(null!), Is.Empty);
            NUnitAssert.That(mapper.MapToEntity(new List<PublicEventCardDto>()), Is.Empty);
        }

        [Test]
        public void MapToEntity_MapsValuesAndMunicipality()
        {
            var mapper = new PublicEventCardMapper();
            var id = Guid.Parse("44444444-4444-4444-4444-444444444444");

            var dtos = new List<PublicEventCardDto>
            {
                new()
                {
                    EntityId = id.ToString(),
                    EntityName = " Event ",
                    ImagePath = " img ",
                    BadgeText = " badge ",
                    Address = " addr ",
                    Date = " date ",
                    MunicipalityData = new MunicipalityForLocalStorageSettingDto { Name = "City", LogoPath = "logo" }
                },
                new()
                {
                    EntityId = "invalid",
                    EntityName = null,
                    ImagePath = null,
                    BadgeText = null,
                    Address = null,
                    Date = null
                }
            };

            var result = mapper.MapToEntity(dtos);

            NUnitAssert.That(result, Has.Count.EqualTo(2));
            NUnitAssert.That(result[0].EntityId, Is.EqualTo(id));
            NUnitAssert.That(result[0].EntityName, Is.EqualTo("Event"));
            NUnitAssert.That(result[0].MunicipalityData, Is.Not.Null);
            NUnitAssert.That(result[1].EntityId, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result[1].ImagePath, Is.EqualTo(string.Empty));
        }
    }
}
