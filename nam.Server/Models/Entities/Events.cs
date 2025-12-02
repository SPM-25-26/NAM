using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nam.Server.Models.Entities
{
    public class Events
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

        // Top-level municipality data (embedded summary)
        public EventMunicipalityData? MunicipalityData { get; set; }

        // Event date (nullable to allow unknown)
        public DateTime? Date { get; set; }

        // One-to-one navigation to detail
        public EventDetail? Detail { get; set; }
    }

    public class EventDetail
    {
        [Key]
        [Required]
        public Guid Identifier { get; set; }

        [Required]
        [MaxLength(500)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Typology { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string PrimaryImage { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Telephone { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Website { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Facebook { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Instagram { get; set; } = string.Empty;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Collections
        public ICollection<EventGalleryImage> Gallery { get; set; } = new List<EventGalleryImage>();
        public ICollection<EventVirtualTour> VirtualTours { get; set; } = new List<EventVirtualTour>();
        public ICollection<EventNeighbor> Neighbors { get; set; } = new List<EventNeighbor>();
        public ICollection<EventTicketAndCost> TicketsAndCosts { get; set; } = new List<EventTicketAndCost>();
        public ICollection<EventAssociatedService> AssociatedServices { get; set; } = new List<EventAssociatedService>();

        // Dates for event period
        public DateTime? Date { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // One-to-one / complex
        public EventNearestCarPark? NearestCarPark { get; set; }
        public EventOrganizer? Organizer { get; set; }
        public EventMunicipalityData? MunicipalityData { get; set; }
    }

    public class EventGalleryImage
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [Required]
        public Guid EventDetailId { get; set; }

        [ForeignKey(nameof(EventDetailId))]
        public EventDetail EventDetail { get; set; } = null!;
    }

    public class EventVirtualTour
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(1000)]
        public string TourUrl { get; set; } = string.Empty;

        [Required]
        public Guid EventDetailId { get; set; }

        [ForeignKey(nameof(EventDetailId))]
        public EventDetail EventDetail { get; set; } = null!;
    }

    public class EventNeighbor
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
        public Guid EventDetailId { get; set; }

        [ForeignKey(nameof(EventDetailId))]
        public EventDetail EventDetail { get; set; } = null!;
    }

    public class EventNearestCarPark
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [MaxLength(1000)]
        public string Address { get; set; } = string.Empty;

        public double Distance { get; set; }

        [Required]
        public Guid EventDetailId { get; set; }

        [ForeignKey(nameof(EventDetailId))]
        public EventDetail EventDetail { get; set; } = null!;
    }

    public class EventOrganizer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(100)]
        public string TaxCode { get; set; } = string.Empty;

        [MaxLength(500)]
        public string LegalName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Website { get; set; } = string.Empty;

        [Required]
        public Guid EventDetailId { get; set; }

        [ForeignKey(nameof(EventDetailId))]
        public EventDetail EventDetail { get; set; } = null!;
    }

    public class EventTicketAndCost
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal PriceSpecificationCurrencyValue { get; set; }

        [MaxLength(10)]
        public string Currency { get; set; } = "EUR";

        [MaxLength(1000)]
        public string ValidityDescription { get; set; } = string.Empty;

        public DateTime? ValidityStartDate { get; set; }
        public DateTime? ValidityEndDate { get; set; }

        [MaxLength(255)]
        public string UserTypeName { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string UserTypeDescription { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string TicketDescription { get; set; } = string.Empty;

        [Required]
        public Guid EventDetailId { get; set; }

        [ForeignKey(nameof(EventDetailId))]
        public EventDetail EventDetail { get; set; } = null!;
    }

    public class EventAssociatedService
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
        public Guid EventDetailId { get; set; }

        [ForeignKey(nameof(EventDetailId))]
        public EventDetail EventDetail { get; set; } = null!;
    }

    public class EventMunicipalityData
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string LogoPath { get; set; } = string.Empty;

        // optional backref
        public Guid? EventsId { get; set; }
    }
}
