using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class ArticleCardMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsEmpty_WhenDtoListNullOrEmpty()
        {
            var mapper = new ArticleCardMapper();

            NUnitAssert.That(mapper.MapToEntity(null!), Is.Empty);
            NUnitAssert.That(mapper.MapToEntity(new List<ArticleCardDto>()), Is.Empty);
        }

        [Test]
        public void MapToEntity_MapsValuesAndDefaults()
        {
            var mapper = new ArticleCardMapper();
            var id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

            var dtos = new List<ArticleCardDto>
            {
                new()
                {
                    entityId = id.ToString(),
                    EntityName = "Article",
                    BadgeText = "Badge",
                    ImagePath = "img.png",
                    Address = "Addr"
                },
                new()
                {
                    entityId = "invalid",
                    EntityName = null!,
                    BadgeText = null!,
                    ImagePath = null!,
                    Address = null
                }
            };

            var result = mapper.MapToEntity(dtos);

            NUnitAssert.That(result, Has.Count.EqualTo(2));
            NUnitAssert.That(result[0].EntityId, Is.EqualTo(id));
            NUnitAssert.That(result[0].EntityName, Is.EqualTo("Article"));
            NUnitAssert.That(result[1].EntityId, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result[1].BadgeText, Is.EqualTo(string.Empty));
        }

        [Test]
        public void MapToEntity_SkipsNullDtos()
        {
            var mapper = new ArticleCardMapper();

            var result = mapper.MapToEntity(new List<ArticleCardDto?> { null!, new ArticleCardDto { entityId = Guid.Empty.ToString(), EntityName = "Name", BadgeText = "Badge", ImagePath = "Img" } }!);

            NUnitAssert.That(result, Has.Count.EqualTo(1));
        }
    }
}
