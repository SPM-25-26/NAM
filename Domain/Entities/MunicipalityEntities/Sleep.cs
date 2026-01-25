using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class SleepCard
    {
        [Key]
        public Guid EntityId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(500)]
        [Embeddable]
        public string EntityName { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Embeddable]
        public string BadgeText { get; set; } = string.Empty;

        [MaxLength(1000)]
        [Embeddable]
        public string Address { get; set; } = string.Empty;

        [Embeddable]
        public SleepCardDetail? Detail { get; set; }
    }

    public class SleepCardDetail
    {
        [Key]
        [Required]
        [MaxLength(255)]
        public Guid Identifier { get; set; } = Guid.NewGuid();

        [MaxLength(500)]
        [Embeddable]
        public string? OfficialName { get; set; }

        [MaxLength(4000)]
        [Embeddable]
        public string? Description { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? Classification { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? Typology { get; set; }

        [MaxLength(1000)]
        public string? PrimaryImage { get; set; }

        public ICollection<string> Gallery { get; set; } = [];
        public ICollection<string> VirtualTours { get; set; } = [];
        public ICollection<string> Services { get; set; } = [];
        public ICollection<string> RoomTypologies { get; set; } = [];

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

        [Embeddable]
        public double Latitude { get; set; }

        [Embeddable]
        public double Longitude { get; set; }

        [Embeddable]
        public List<FeatureCardRelationship<SleepCardDetail>> Neighbors { get; set; } = [];

        [Embeddable]
        public NearestCarPark? NearestCarPark { get; set; }

        [Embeddable]
        public OpeningHoursSpecification? OpeningHours { get; set; }

        [Embeddable]
        public TemporaryClosure? TemporaryClosure { get; set; }

        [Embeddable]
        public Owner? Owner { get; set; }

        [Embeddable]
        public Booking? Booking { get; set; }

        [MaxLength(500)]
        [Embeddable]
        public string? ShortAddress { get; set; }

        [Embeddable]
        public List<Offer>? Offers { get; set; } = [];

        [Embeddable]
        public ICollection<AssociatedService> AssociatedServices { get; set; } = [];

        [Embeddable]
        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }
}