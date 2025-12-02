using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nam.Server.Models.Entities
{
    public class ArtCulture
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

        public ArtCultureDetail? Detail { get; set; }
    }


    public class ArtCultureDetail
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

        // Navigation properties per le relazioni con le entità figlie
        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<CulturalProject> CulturalProjects { get; set; } = new List<CulturalProject>();
        public ICollection<Catalogue> Catalogues { get; set; } = new List<Catalogue>();
        public ICollection<CreativeWork> CreativeWorks { get; set; } = new List<CreativeWork>();
        public ICollection<GalleryImage> Gallery { get; set; } = new List<GalleryImage>();
        public ICollection<VirtualTour> VirtualTours { get; set; } = new List<VirtualTour>();
        public ICollection<Neighbor> Neighbors { get; set; } = new List<Neighbor>();
        public ICollection<AssociatedService> AssociatedServices { get; set; } = new List<AssociatedService>();

        // Relazioni one-to-one
        public NearestCarPark? NearestCarPark { get; set; }
        public Site? Site { get; set; }
        public MunicipalityData? MunicipalityData { get; set; }
    }

    public class Service
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string ArtCultureDetailId { get; set; } = string.Empty;

        [ForeignKey(nameof(ArtCultureDetailId))]
        public ArtCultureDetail ArtCultureDetail { get; set; } = null!;
    }

    public class CulturalProject
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Url { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string ArtCultureDetailId { get; set; } = string.Empty;

        [ForeignKey(nameof(ArtCultureDetailId))]
        public ArtCultureDetail ArtCultureDetail { get; set; } = null!;
    }

    public class Catalogue
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string WebsiteUrl { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string ArtCultureDetailId { get; set; } = string.Empty;

        [ForeignKey(nameof(ArtCultureDetailId))]
        public ArtCultureDetail ArtCultureDetail { get; set; } = null!;
    }

    public class CreativeWork
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Type { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Url { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string ArtCultureDetailId { get; set; } = string.Empty;

        [ForeignKey(nameof(ArtCultureDetailId))]
        public ArtCultureDetail ArtCultureDetail { get; set; } = null!;
    }

    public class GalleryImage
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string ArtCultureDetailId { get; set; } = string.Empty;

        [ForeignKey(nameof(ArtCultureDetailId))]
        public ArtCultureDetail ArtCultureDetail { get; set; } = null!;
    }

    public class VirtualTour
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(1000)]
        public string TourUrl { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string ArtCultureDetailId { get; set; } = string.Empty;

        [ForeignKey(nameof(ArtCultureDetailId))]
        public ArtCultureDetail ArtCultureDetail { get; set; } = null!;
    }

    public class Neighbor
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string EntityId { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [MaxLength(500)]
        public string ExtraInfo { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string ArtCultureDetailId { get; set; } = string.Empty;

        [ForeignKey(nameof(ArtCultureDetailId))]
        public ArtCultureDetail ArtCultureDetail { get; set; } = null!;
    }

    public class NearestCarPark
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [MaxLength(1000)]
        public string Address { get; set; } = string.Empty;

        public double Distance { get; set; }
    }

    public class Site
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Identifier { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string OfficialName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string ArtCultureDetailId { get; set; } = string.Empty;

        [ForeignKey(nameof(ArtCultureDetailId))]
        public ArtCultureDetail ArtCultureDetail { get; set; } = null!;
    }

    public class MunicipalityData
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string LogoPath { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string ArtCultureDetailId { get; set; } = string.Empty;

        [ForeignKey(nameof(ArtCultureDetailId))]
        public ArtCultureDetail ArtCultureDetail { get; set; } = null!;
    }

    public class AssociatedService
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Identifier { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string ArtCultureDetailId { get; set; } = string.Empty;

        [ForeignKey(nameof(ArtCultureDetailId))]
        public ArtCultureDetail ArtCultureDetail { get; set; } = null!;
    }

}
