using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class EatAndDrinkCard
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
        public EatAndDrinkDetail? Detail { get; set; }
    }

    public class EatAndDrinkDetail
    {
        [Key]
        [Required]
        [MaxLength(255)]
        public Guid Identifier { get; set; } = Guid.NewGuid();

        [MaxLength(1000)]
        public string? PrimaryImagePath { get; set; }

        [MaxLength(500)]
        [Embeddable]
        public string? OfficialName { get; set; }

        [MaxLength(1000)]
        [Embeddable]
        public string? Address { get; set; }

        [MaxLength(4000)]
        [Embeddable]
        public string? Description { get; set; }

        [MaxLength(320)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Telephone { get; set; }

        [MaxLength(500)]
        public string? Facebook { get; set; }

        [MaxLength(500)]
        public string? Instagram { get; set; }

        [MaxLength(1000)]
        public string? Website { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? Type { get; set; }

        public ICollection<string> Gallery { get; set; } = [];
        public ICollection<string> VirtualTours { get; set; } = [];

        [Embeddable]
        public double Latitude { get; set; }

        [Embeddable]
        public double Longitude { get; set; }

        [Embeddable]
        public ICollection<OntoremaService> Services { get; set; } = [];

        [Embeddable]
        public List<FeatureCardRelationship<EatAndDrinkDetail>> Neighbors { get; set; } = [];

        [Embeddable]
        public NearestCarPark? NearestCarPark { get; set; }

        [Embeddable]
        public OpeningHoursSpecification? OpeningHours { get; set; }

        [Embeddable]
        public TemporaryClosure? TemporaryClosure { get; set; }

        [Embeddable]
        public Booking? Booking { get; set; }

        public ICollection<string> DietaryNeeds { get; set; } = [];

        [Embeddable]
        public ICollection<TypicalProductMobile> TypicalProducts { get; set; } = [];

        [Embeddable]
        public Owner? Owner { get; set; }

        [Embeddable]
        public ICollection<AssociatedService> AssociatedServices { get; set; } = [];

        [Embeddable]
        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }

    public class OntoremaService
    {

        [MaxLength(255)]
        [Embeddable]
        public string? Name { get; set; }

        [MaxLength(1000)]
        [Embeddable]
        public string? Description { get; set; }
    }

    public class TypicalProductMobile
    {

        [MaxLength(255)]
        [Embeddable]
        public string? Name { get; set; }

        [MaxLength(1000)]
        [Embeddable]
        public string? Description { get; set; }
    }
}