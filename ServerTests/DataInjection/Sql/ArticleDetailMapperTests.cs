using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class ArticleDetailMapperTests
    {
        [Test]
        public void MapToEntity_ReturnsDefaults_WhenDtoNull()
        {
            var mapper = new ArticleDetailMapper();

            var result = mapper.MapToEntity(null!);

            NUnitAssert.That(result.Identifier, Is.Not.EqualTo(Guid.Empty));
            NUnitAssert.That(result.Title, Is.EqualTo(string.Empty));
            NUnitAssert.That(result.Themes, Is.Empty);
            NUnitAssert.That(result.MunicipalityData, Is.Not.Null);
        }

        [Test]
        public void MapToEntity_MapsParagraphsThemesAndFallbacks()
        {
            var mapper = new ArticleDetailMapper();
            var identifier = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
            var updatedAt = new DateTime(2024, 1, 5);

            var dto = new ArticleDetailDto
            {
                Identifier = identifier.ToString(),
                Title = "Title",
                Script = "Script",
                ImagePath = "img.png",
                UpdatedAt = updatedAt,
                Themes = new List<string> { "Theme", null! },
                Paragraphs = new List<ParagraphDto>
                {
                    new()
                    {
                        Position = 1,
                        Title = "Par",
                        Script = "Body"
                    },
                    null!
                }!,
                MunicipalityData = new MunicipalityForLocalStorageSetting { Name = "City", LogoPath = "logo" }
            };

            var result = mapper.MapToEntity(dto);

            NUnitAssert.That(result.Identifier, Is.EqualTo(identifier));
            NUnitAssert.That(result.Paragraphs, Has.Count.EqualTo(1));
            NUnitAssert.That(result.Paragraphs[0].Title, Is.EqualTo("Par"));
            NUnitAssert.That(result.Themes, Has.Count.EqualTo(2));
            NUnitAssert.That(result.Themes, Is.Not.Null);
            NUnitAssert.That(result.Themes![0], Is.EqualTo("Theme"));
            NUnitAssert.That(result.UpdatedAt, Is.EqualTo(updatedAt));
            NUnitAssert.That(result.MunicipalityData, Is.Not.Null);
        }
    }
}
