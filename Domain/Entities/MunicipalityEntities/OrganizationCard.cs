using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class OrganizationCard
    {
        [Key]
        [Embeddable]
        public string? TaxCode { get; set; }

        [MaxLength(500)]
        [Embeddable]
        public string? EntityName { get; set; }

        [MaxLength(1000)]
        public string? ImagePath { get; set; }

        [MaxLength(100)]
        [Embeddable]
        public string? BadgeText { get; set; }

        [MaxLength(1000)]
        [Embeddable]
        public string? Address { get; set; }

        [Embeddable]
        public OrganizationMobileDetail? Detail { get; set; }
    }

    public class OrganizationMobileDetail
    {
        [Key]
        [MaxLength(100)]
        [Embeddable]
        public string? TaxCode { get; set; }

        [MaxLength(500)]
        [Embeddable]
        public string? LegalName { get; set; }

        [MaxLength(1000)]
        public string? PrimaryImagePath { get; set; }

        [MaxLength(500)]
        [Embeddable]
        public string? Type { get; set; }

        [MaxLength(1000)]
        [Embeddable]
        public string? Address { get; set; }

        [MaxLength(4000)]
        [Embeddable]
        public string? Description { get; set; }

        [MaxLength(500)]
        [Embeddable]
        public string? MainFunction { get; set; }

        public DateTime? FoundationDate { get; set; }

        [MaxLength(200)]
        [Embeddable]
        public string? LegalStatus { get; set; }

        public List<string>? Gallery { get; set; }

        [MaxLength(320)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Telephone { get; set; }

        [MaxLength(1000)]
        public string? Website { get; set; }

        [MaxLength(200)]
        public string? Instagram { get; set; }

        [MaxLength(200)]
        public string? Facebook { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Embeddable]
        public List<FeatureCardRelationship<OrganizationMobileDetail>>? Neighbors { get; set; } = [];

        [Embeddable]
        public NearestCarPark? NearestCarPark { get; set; }

        [Embeddable]
        public List<OwnedPoi>? OwnedPoi { get; set; } = [];

        [Embeddable]
        public List<Offer>? Offers { get; set; } = [];

        [Embeddable]
        public List<PublicEventCard>? Events { get; set; } = [];

        [Embeddable]
        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }

    public class OwnedPoi
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(500)]
        [Embeddable]
        public string? OfficialName { get; set; }

        [MaxLength(1000)]
        public string? ImagePath { get; set; }

        [MaxLength(200)]
        [Embeddable]
        public string? Category { get; set; }
    }
}
