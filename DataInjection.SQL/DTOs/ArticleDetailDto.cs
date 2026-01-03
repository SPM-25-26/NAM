using Domain.Entities.MunicipalityEntities;

namespace DataInjection.DTOs
{
    public class ArticleDetailDto
    {
        public required string Identifier { get; set; }

        public required string Title { get; set; }

        public required string Script { get; set; }

        public string? Region { get; set; }

        public string? Subtitle { get; set; }

        public string? TimeToRead { get; set; }

        public required string ImagePath { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<string>? Themes { get; set; } = [];

        public List<ParagraphDto> Paragraphs { get; set; } = [];

        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }
}
