using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class OrganizationCardMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsEmpty_WhenDtoListNullOrEmpty()
        {
            var mapper = new OrganizationCardMapper();

            NUnitAssert.That(mapper.MapToEntity(null!), Is.Empty);
            NUnitAssert.That(mapper.MapToEntity(new List<OrganizationCardDto>()), Is.Empty);
        }

        [Test]
        public void MapToEntity_MapsValuesAndTrims()
        {
            var mapper = new OrganizationCardMapper();

            var dtos = new List<OrganizationCardDto>
            {
                new() { EntityId = "TAX", EntityName = " Name ", ImagePath = " img ", BadgeText = " Badge ", Address = " Addr " },
                new() { EntityId = "TAX2", EntityName = " ", ImagePath = null, BadgeText = null, Address = null }
            };

            var result = mapper.MapToEntity(dtos);

            NUnitAssert.That(result, Has.Count.EqualTo(2));
            NUnitAssert.That(result[0].TaxCode, Is.EqualTo("TAX"));
            NUnitAssert.That(result[0].EntityName, Is.EqualTo("Name"));
            NUnitAssert.That(result[1].EntityName, Is.Null);
            NUnitAssert.That(result[1].ImagePath, Is.Null);
        }
    }
}
