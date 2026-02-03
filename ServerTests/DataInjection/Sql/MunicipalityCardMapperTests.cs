using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class MunicipalityCardMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsEmpty_WhenDtoListNullOrEmpty()
        {
            var mapper = new MunicipalityCardMapper();

            NUnitAssert.That(mapper.MapToEntity(null!), Is.Empty);
            NUnitAssert.That(mapper.MapToEntity(new List<MunicipalityCardDto>()), Is.Empty);
        }

        [Test]
        public void MapToEntity_MapsValuesAndTrims()
        {
            var mapper = new MunicipalityCardMapper();

            var dtos = new List<MunicipalityCardDto>
            {
                new() { LegalName = " Name ", ImagePath = " img.png " },
                new() { LegalName = " ", ImagePath = null }
            };

            var result = mapper.MapToEntity(dtos);

            NUnitAssert.That(result, Has.Count.EqualTo(2));
            NUnitAssert.That(result[0].LegalName, Is.EqualTo("Name"));
            NUnitAssert.That(result[0].ImagePath, Is.EqualTo("img.png"));
            NUnitAssert.That(result[1].LegalName, Is.Null);
            NUnitAssert.That(result[1].ImagePath, Is.Null);
        }
    }
}
