using System.ComponentModel.DataAnnotations;

namespace nam.Server.Models.Entities.MunicipalityEntities
{
    public class ArticleCard
    {
        [Key]
        public Guid EntityId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string EntityName { get; set; }

        [Required]
        [MaxLength(50)]
        public required string BadgeText { get; set; }

        [Required]
        [MaxLength(500)]
        public required string ImagePath { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        public ArticleDetail? Detail { get; set; }
    }

    public class ArticleDetail
    {
        [Key]
        public Guid Identifier { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }

        [Required]
        public required string Script { get; set; }

        [MaxLength(100)]
        public string? Region { get; set; }

        [MaxLength(255)]
        public string? Subtitle { get; set; }

        [MaxLength(50)]
        public string? TimeToRead { get; set; }

        [Required]
        [MaxLength(500)]
        public required string ImagePath { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public List<string>? Themes { get; set; } = [];

        public List<Paragraph> Paragraphs { get; set; } = [];

        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }

    public class Paragraph
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        //public ArticleDetail? ArticleDetail { get; set; }

        [Required]
        public int Position { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }

        [Required]
        public required string Script { get; set; }

        [MaxLength(255)]
        public string? Subtitle { get; set; }

        [MaxLength(100)]
        public string? Region { get; set; }

        [MaxLength(100)]
        public string? ReferenceIdentifier { get; set; }

        [MaxLength(255)]
        public string? ReferenceName { get; set; }

        [MaxLength(100)]
        public string? ReferenceCategory { get; set; }

        [MaxLength(500)]
        public string? ReferenceImagePath { get; set; }

        public double? ReferenceLatitude { get; set; }

        public double? ReferenceLongitude { get; set; }
    }
}