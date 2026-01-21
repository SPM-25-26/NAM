using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class ServiceCard
    {
        [Key]
        public Guid EntityId { get; set; } = Guid.NewGuid();

        [MaxLength(500)]
        [Embeddable]
        public string EntityName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [MaxLength(100)]
        [Embeddable]
        public string BadgeText { get; set; } = string.Empty;

        [MaxLength(1000)]
        [Embeddable]
        public string Address { get; set; } = string.Empty;

        [Embeddable]
        public ServiceDetail? Detail { get; set; }
    }

    public class ServiceDetail
    {
        [Key]
        [Required]
        [MaxLength(255)]
        public Guid Identifier { get; set; } = Guid.NewGuid();

        [MaxLength(500)]
        [Embeddable]
        public string? Name { get; set; }

        [MaxLength(1000)]
        [Embeddable]
        public string? Address { get; set; }

        [MaxLength(4000)]
        [Embeddable]
        public string? Description { get; set; }

        [Embeddable]
        public int? SpacesForDisabled { get; set; }

        [Embeddable]
        public int? PayingParkingSpaces { get; set; }

        [Embeddable]
        public int? AvailableParkingSpaces { get; set; }

        [Embeddable]
        public int? PostiAutoVenduti { get; set; }

        [Embeddable]
        public int? TotalNumberOfCarParkSpaces { get; set; }

        [Embeddable]
        public double Latitude { get; set; }

        [Embeddable]
        public double Longitude { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? Typology { get; set; }

        [MaxLength(1000)]
        public string? PrimaryImage { get; set; }

        public ICollection<string> Gallery { get; set; } = [];

        [MaxLength(320)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Telephone { get; set; }

        [MaxLength(1000)]
        public string? Website { get; set; }

        [MaxLength(500)]
        public string? Instagram { get; set; }

        [MaxLength(500)]
        public string? Facebook { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? Price { get; set; }

        [MaxLength(2048)]
        [Url]
        public string? ReservationUrl { get; set; }

        [Embeddable]
        public List<FeatureCardRelationship<ServiceDetail>> Neighbors { get; set; } = [];

        [Embeddable]
        public NearestCarPark? NearestCarPark { get; set; }

        [Embeddable]
        public OpeningHoursSpecification? OpeningHours { get; set; }

        [Embeddable]
        public TemporaryClosure? TemporaryClosure { get; set; }

        [Embeddable]
        public Booking? Booking { get; set; }

        [Embeddable]
        public List<ServiceLocationRelationship<ServiceDetail>> Locations { get; set; } = [];

        [Embeddable]
        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }

        [Embeddable]
        public ICollection<AssociatedService> AssociatedServices { get; set; } = [];
    }
}