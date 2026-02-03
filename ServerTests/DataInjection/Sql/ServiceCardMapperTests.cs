using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class ServiceCardMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsEmpty_WhenDtoListIsNull()
        {
            var mapper = new ServiceCardMapper();

            var result = mapper.MapToEntity(null!);

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public void MapToEntity_MapsValidValuesAndDefaults()
        {
            var mapper = new ServiceCardMapper();
            var id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            var dtos = new List<ServiceCardDto>
            {
                new()
                {
                    EntityId = id.ToString(),
                    EntityName = "Service Name",
                    ImagePath = "image.png",
                    BadgeText = "Featured",
                    Address = "Main Street"
                },
                new()
                {
                    EntityId = "not-a-guid",
                    EntityName = null!,
                    ImagePath = null!,
                    BadgeText = null!,
                    Address = null!
                }
            };

            var result = mapper.MapToEntity(dtos);

            NUnitAssert.That(result, Has.Count.EqualTo(2));
            NUnitAssert.That(result[0].EntityId, Is.EqualTo(id));
            NUnitAssert.That(result[0].EntityName, Is.EqualTo("Service Name"));
            NUnitAssert.That(result[0].ImagePath, Is.EqualTo("image.png"));
            NUnitAssert.That(result[0].BadgeText, Is.EqualTo("Featured"));
            NUnitAssert.That(result[0].Address, Is.EqualTo("Main Street"));
            NUnitAssert.That(result[1].EntityId, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result[1].EntityName, Is.EqualTo(string.Empty));
            NUnitAssert.That(result[1].ImagePath, Is.EqualTo(string.Empty));
            NUnitAssert.That(result[1].BadgeText, Is.EqualTo(string.Empty));
            NUnitAssert.That(result[1].Address, Is.EqualTo(string.Empty));
        }

        [Test]
        public void MapToEntity_SkipsNullDtos()
        {
            var mapper = new ServiceCardMapper();

            var dtos = new List<ServiceCardDto>
            {
                new()
                {
                    EntityId = Guid.Empty.ToString(),
                    EntityName = "Valid",
                    ImagePath = "img.png",
                    BadgeText = "Badge",
                    Address = "Address"
                },
                null!
            };

            var result = mapper.MapToEntity(dtos);

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].EntityName, Is.EqualTo("Valid"));
        }
    }
}
