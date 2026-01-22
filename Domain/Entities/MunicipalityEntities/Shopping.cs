using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class ShoppingCard
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

        [MaxLength(500)]
        [Embeddable]
        public string Address { get; set; } = string.Empty;

        [Embeddable]
        public ShoppingCardDetail? Detail { get; set; }
    }

    public class ShoppingCardDetail
    {
        [Key]
        [Required]
        [MaxLength(255)]
        public Guid Identifier { get; set; } = Guid.NewGuid();

        [MaxLength(500)]
        [Embeddable]
        public string? OfficialName { get; set; }

        [MaxLength(1000)]
        [Embeddable]
        public string? Address { get; set; }

        [MaxLength(4000)]
        [Embeddable]
        public string? Description { get; set; }

        [MaxLength(1000)]
        public string? ImagePath { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? PoiCategory { get; set; }

        [Embeddable]
        public double Latitude { get; set; }

        [Embeddable]
        public double Longitude { get; set; }

        [Embeddable]
        public List<FeatureCardRelationship<ShoppingCardDetail>> Neighbors { get; set; } = [];

        [Embeddable]
        public NearestCarPark? NearestCarPark { get; set; }

        [MaxLength(320)]
        public string? Email { get; set; }

        [MaxLength(1000)]
        public string? Website { get; set; }

        [MaxLength(500)]
        public string? Facebook { get; set; }

        [MaxLength(500)]
        public string? Instagram { get; set; }

        [MaxLength(50)]
        public string? Telephone { get; set; }

        [Embeddable]
        public Owner? Owner { get; set; }

        public ICollection<string> Gallery { get; set; } = [];
        public ICollection<string> VirtualTours { get; set; } = [];

        [Embeddable]
        public ICollection<PointOfSaleService> Services { get; set; } = [];

        [Embeddable]
        public ICollection<AssociatedService> AssociatedServices { get; set; } = [];

        [Embeddable]
        public ICollection<TypicalProduct> SellingTypicalProducts { get; set; } = [];

        [Embeddable]
        public OpeningHoursSpecification? OpeningHours { get; set; }

        [Embeddable]
        public TemporaryClosure? TemporaryClosure { get; set; }

        [Embeddable]
        public Booking? Booking { get; set; }

        [Embeddable]
        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }

    public class Owner
    {
        [Key]
        [Embeddable]
        public string TaxCode { get; set; }

        [Embeddable]
        public string? LegalName { get; set; }

        [MaxLength(1000)]
        public string? WebSite { get; set; }
    }

    public class PointOfSaleService
    {
        [MaxLength(255)]
        [Embeddable]
        public string? Name { get; set; }

        [MaxLength(1000)]
        [Embeddable]
        public string? Description { get; set; }
    }
}