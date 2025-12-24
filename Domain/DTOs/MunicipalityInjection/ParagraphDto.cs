namespace Domain.DTOs.MunicipalityInjection
{
    public class ParagraphDto
    {
        public int Position { get; set; }

        public required string Title { get; set; }

        public required string Script { get; set; }

        public string? Subtitle { get; set; }

        public string? Region { get; set; }

        public string? ReferenceIdentifier { get; set; }

        public string? ReferenceName { get; set; }

        public string? ReferenceCategory { get; set; }

        public string? ReferenceImagePath { get; set; }

        public double? ReferenceLatitude { get; set; }

        public double? ReferenceLongitude { get; set; }
    }
}
