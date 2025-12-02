using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nam.Server.Models.Entities
{
    public class EntertainmentLeisure
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

        // One-to-one navigation to detail
        public EntertainmentLeisureDetail? Detail { get; set; }
    }

    public class EntertainmentLeisureDetail
    {
        [Key]
        [Required]
        public Guid Identifier { get; set; }

        [Required]
        [MaxLength(500)]
        public string OfficialName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string PrimaryImage { get; set; } = string.Empty;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Collections
        public ICollection<EntertainmentGalleryImage> Gallery { get; set; } = new List<EntertainmentGalleryImage>();
        public ICollection<EntertainmentVirtualTour> VirtualTours { get; set; } = new List<EntertainmentVirtualTour>();
        public ICollection<EntertainmentNeighbor> Neighbors { get; set; } = new List<EntertainmentNeighbor>();
        public ICollection<EntertainmentAssociatedService> AssociatedServices { get; set; } = new List<EntertainmentAssociatedService>();

        // One-to-one / complex
        public EntertainmentNearestCarPark? NearestCarPark { get; set; }
        public EntertainmentMunicipalityData? MunicipalityData { get; set; }
    }

    public class EntertainmentGalleryImage
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [Required]
        public Guid EntertainmentLeisureDetailId { get; set; }

        [ForeignKey(nameof(EntertainmentLeisureDetailId))]
        public EntertainmentLeisureDetail EntertainmentLeisureDetail { get; set; } = null!;
    }

    public class EntertainmentVirtualTour
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(1000)]
        public string TourUrl { get; set; } = string.Empty;

        [Required]
        public Guid EntertainmentLeisureDetailId { get; set; }

        [ForeignKey(nameof(EntertainmentLeisureDetailId))]
        public EntertainmentLeisureDetail EntertainmentLeisureDetail { get; set; } = null!;
    }

    public class EntertainmentNeighbor
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

        [MaxLength(1000)]
        public string ExtraInfo { get; set; } = string.Empty;

        [Required]
        public Guid EntertainmentLeisureDetailId { get; set; }

        [ForeignKey(nameof(EntertainmentLeisureDetailId))]
        public EntertainmentLeisureDetail EntertainmentLeisureDetail { get; set; } = null!;
    }

    public class EntertainmentNearestCarPark
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [MaxLength(1000)]
        public string Address { get; set; } = string.Empty;

        public double Distance { get; set; }

        [Required]
        public Guid EntertainmentLeisureDetailId { get; set; }

        [ForeignKey(nameof(EntertainmentLeisureDetailId))]
        public EntertainmentLeisureDetail EntertainmentLeisureDetail { get; set; } = null!;
    }

    public class EntertainmentAssociatedService
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public string Identifier { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [Required]
        public Guid EntertainmentLeisureDetailId { get; set; }

        [ForeignKey(nameof(EntertainmentLeisureDetailId))]
        public EntertainmentLeisureDetail EntertainmentLeisureDetail { get; set; } = null!;
    }

    public class EntertainmentMunicipalityData
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string LogoPath { get; set; } = string.Empty;

        [Required]
        public Guid EntertainmentLeisureDetailId { get; set; }

        [ForeignKey(nameof(EntertainmentLeisureDetailId))]
        public EntertainmentLeisureDetail EntertainmentLeisureDetail { get; set; } = null!;
    }

}
