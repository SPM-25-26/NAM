using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nam.Server.Models.Entities
{
    public class Article
    {
        [Key]
        [Required]
        public Guid EntityId { get; set; }

        [Required]
        [MaxLength(500)]
        public string EntityName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [MaxLength(100)]
        public string BadgeText { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Address { get; set; } = string.Empty;

        public ArticleDetail? Detail { get; set; }
    }


    public class ArticleDetail
    {
        [Key]
        [Required]
        public Guid Identifier { get; set; }

        [Required]
        [MaxLength(500)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string Script { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Region { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Subtitle { get; set; } = string.Empty;

        [MaxLength(50)]
        public string TimeToRead { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }

        // Navigation properties per le relazioni con le entità figlie
        public ICollection<ArticleParagraph> Paragraphs { get; set; } = new List<ArticleParagraph>();
        public ICollection<ArticleTheme> Themes { get; set; } = new List<ArticleTheme>();

        // Relazione one-to-one
        public ArticleMunicipalityData? MunicipalityData { get; set; }
    }

    public class ArticleParagraph
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(500)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string Script { get; set; } = string.Empty;

        public int Position { get; set; }

        [MaxLength(500)]
        public string Subtitle { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Region { get; set; } = string.Empty;

        [MaxLength(255)]
        public string ReferenceIdentifier { get; set; } = string.Empty;

        [MaxLength(255)]
        public string ReferenceName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ReferenceCategory { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ReferenceImagePath { get; set; } = string.Empty;

        public double ReferenceLongitude { get; set; }

        public double ReferenceLatitude { get; set; }

        [Required]
        public Guid ArticleDetailId { get; set; }

        [ForeignKey(nameof(ArticleDetailId))]
        public ArticleDetail ArticleDetail { get; set; } = null!;
    }

    public class ArticleTheme
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Theme { get; set; } = string.Empty;

        [Required]
        public Guid ArticleDetailId { get; set; }

        [ForeignKey(nameof(ArticleDetailId))]
        public ArticleDetail ArticleDetail { get; set; } = null!;
    }

    public class ArticleMunicipalityData
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string LogoPath { get; set; } = string.Empty;

        [Required]
        public Guid ArticleDetailId { get; set; }

        [ForeignKey(nameof(ArticleDetailId))]
        public ArticleDetail ArticleDetail { get; set; } = null!;
    }

}