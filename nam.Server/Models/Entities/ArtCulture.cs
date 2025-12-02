using System.ComponentModel.DataAnnotations;

namespace nam.Server.Models.Entities
{
    public class ArtCultureNatureCard
    {
        [Key]
        public Guid EntityId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(500)]
        public string EntityName { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string BadgeText { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Address { get; set; } = string.Empty;

        public ArtCultureNatureDetail? Detail { get; set; }
    }


    public class ArtCultureNatureDetail
    {
        [Key]
        [Required]
        [MaxLength(255)]
        public Guid Identifier { get; set; }


        [Required]
        [MaxLength(500)]
        public string OfficialName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string PrimaryImagePath { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string FullAddress { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Type { get; set; } = string.Empty;

        [MaxLength(500)]
        public string SubjectDiscipline { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string Description { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Telephone { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Website { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Instagram { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Facebook { get; set; } = string.Empty;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public ICollection<CulturalSiteService> Services { get; set; } = new List<CulturalSiteService>();
        public ICollection<CulturalProject> CulturalProjects { get; set; } = new List<CulturalProject>();
        public ICollection<Catalogue> Catalogues { get; set; } = new List<Catalogue>();
        public ICollection<CreativeWorkMobile> CreativeWorks { get; set; } = new List<CreativeWorkMobile>();
        public ICollection<String> Gallery { get; set; } = new List<String>();
        public ICollection<String> VirtualTours { get; set; } = new List<String>();
        public ICollection<FeatureCard> Neighbors { get; set; } = new List<FeatureCard>();
        public ICollection<AssociatedService> AssociatedServices { get; set; } = new List<AssociatedService>();

        public NearestCarPark? NearestCarPark { get; set; }
        public SiteCard? Site { get; set; }
        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }

    public class CulturalSiteService
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
    }

    public class CulturalProject
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(2048)]
        [Url]
        public required string Url { get; set; }
    }

    public class Catalogue
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(2048)]
        [Url]
        public required string WebsiteUrl { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }
    }

    public class CreativeWorkMobile
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public required string Type { get; set; }

        [Required]
        [MaxLength(2048)]
        [Url]
        public required string Url { get; set; }
    }

    public class FeatureCard
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(100)]
        public string? EntityId { get; set; }

        [MaxLength(255)]
        public string? Title { get; set; }

        public MobileCategory Category { get; set; }

        [MaxLength(500)]
        public string? ImagePath { get; set; }

        public string? ExtraInfo { get; set; }
    }

    public class AssociatedService
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public required string Identifier { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public required string ImagePath { get; set; }
    }

    public class NearestCarPark
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        public double Distance { get; set; }
    }

    public class SiteCard
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public required string Identifier { get; set; }

        [Required]
        [MaxLength(255)]
        public required string OfficialName { get; set; }

        [Required]
        [MaxLength(500)]
        public required string ImagePath { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Category { get; set; }
    }

    public enum MobileCategory
    {
        Sleep,
        EatAndDrink,
        Events,
        ArtCulture,
        Nature,
        TypicalProducts,
        Routes,
        Services,
        EntertainmentLeisure,
        Organizations,
        Articles,
        Shopping
    }

    public class MobileCategoryDetail
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public MobileCategory Category { get; set; }

        [MaxLength(500)]
        public string? ImagePath { get; set; }

        [MaxLength(100)]
        public string? Label { get; set; }
    }
}
