using Domain.Entities.MunicipalityEntities;
using Infrastructure.Extensions;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.DataInjection.Qdrant
{
    [TestFixture]
    public class EmbeddingExtensionsTests
    {
        [Test]
        public void ToEmbeddingString_ReturnsEmpty_WhenEntityNull()
        {
            ArticleCard? entity = null;

            var result = entity.ToEmbeddingString();

            NUnitAssert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void ToEmbeddingString_IncludesEmbeddablePropertiesAndFormatsCollections()
        {
            var entity = new ArticleCard
            {
                EntityId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                EntityName = "Test Article",
                BadgeText = "Featured",
                ImagePath = "/img.png",
                Address = "Main Street",
                Detail = new ArticleDetail
                {
                    Identifier = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Title = "Title",
                    Script = "Script",
                    Region = "Region",
                    Subtitle = "Subtitle",
                    TimeToRead = "5 min",
                    ImagePath = "/detail.png",
                    UpdatedAt = new DateTime(2024, 1, 15, 10, 30, 45),
                    Themes = ["Culture", "History"],
                    Paragraphs =
                    [
                        new Paragraph
                        {
                            Title = "Paragraph",
                            Position = 1,
                            Script = "Paragraph script",
                            Subtitle = "Paragraph subtitle",
                            Region = "Paragraph region",
                            ReferenceName = "Reference",
                            ReferenceCategory = "Category",
                            ReferenceLatitude = 45.4,
                            ReferenceLongitude = 9.2
                        }
                    ],
                    MunicipalityData = new MunicipalityForLocalStorageSetting
                    {
                        Name = "Milano",
                        LogoPath = "logo.png"
                    }
                }
            };

            var result = entity.ToEmbeddingString();

            NUnitAssert.That(result, Does.Contain("EntityName: \"Test Article\""));
            NUnitAssert.That(result, Does.Contain("BadgeText: \"Featured\""));
            NUnitAssert.That(result, Does.Contain("Address: \"Main Street\""));
            NUnitAssert.That(result, Does.Contain("UpdatedAt: \"2024-01-15 10:30:45\""));
            NUnitAssert.That(result, Does.Contain("Themes:"));
            NUnitAssert.That(result, Does.Contain("- \"Culture\""));
            NUnitAssert.That(result, Does.Contain("Paragraphs:"));
            NUnitAssert.That(result, Does.Contain("Title: \"Paragraph\""));
            NUnitAssert.That(result, Does.Not.Contain("ReferenceLatitude: 45.4"));
            NUnitAssert.That(result, Does.Contain("MunicipalityData:"));
            NUnitAssert.That(result, Does.Contain("Name: \"Milano\""));
        }
    }
}
