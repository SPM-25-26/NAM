using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class ArticleCard
    {
        [Key]
        public Guid EntityId { get; set; }

        [Required]
        [MaxLength(255)]
        [Embeddable]
        public required string EntityName { get; set; }

        [Required]
        [MaxLength(50)]
        [Embeddable]
        public required string BadgeText { get; set; }

        [Required]
        [MaxLength(500)]
        public required string ImagePath { get; set; }

        [MaxLength(500)]
        [Embeddable]
        public string? Address { get; set; }

        [Embeddable]
        public ArticleDetail? Detail { get; set; }
    }

    public class ArticleDetail
    {
        [Key]
        public Guid Identifier { get; set; }

        [Required]
        [MaxLength(255)]
        [Embeddable]
        public required string Title { get; set; }

        [Required]
        [Embeddable]
        public required string Script { get; set; }

        [MaxLength(100)]
        [Embeddable]
        public string? Region { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? Subtitle { get; set; }

        [MaxLength(50)]
        [Embeddable]
        public string? TimeToRead { get; set; }

        [Required]
        [MaxLength(500)]
        public required string ImagePath { get; set; }

        [Required]
        [Embeddable]
        public DateTime UpdatedAt { get; set; }

        [Embeddable]
        public List<string>? Themes { get; set; } = [];

        [Embeddable]
        public List<Paragraph> Paragraphs { get; set; } = [];

        [Embeddable]
        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }

    public class Paragraph
    {
        [Key]
        [MaxLength(255)]
        [Embeddable]
        public required string Title { get; set; }

        [Required]
        public int Position { get; set; }

        [Required]
        [Embeddable]
        public required string Script { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? Subtitle { get; set; }

        [MaxLength(100)]
        [Embeddable]
        public string? Region { get; set; }

        [MaxLength(100)]
        public string? ReferenceIdentifier { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? ReferenceName { get; set; }

        [MaxLength(100)]
        [Embeddable]
        public string? ReferenceCategory { get; set; }

        [MaxLength(500)]
        public string? ReferenceImagePath { get; set; }

        [Embeddable]
        public double? ReferenceLatitude { get; set; }

        [Embeddable]
        public double? ReferenceLongitude { get; set; }
    }
}