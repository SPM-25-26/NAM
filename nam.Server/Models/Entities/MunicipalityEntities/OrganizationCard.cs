using System.ComponentModel.DataAnnotations;

namespace nam.Server.Models.Entities.MunicipalityEntities
{
    public class OrganizationCard
    {
        [Key]
        public string? TaxCode { get; set; }

        [MaxLength(500)]
        public string? EntityName { get; set; }

        [MaxLength(1000)]
        public string? ImagePath { get; set; }

        [MaxLength(100)]
        public string? BadgeText { get; set; }

        [MaxLength(1000)]
        public string? Address { get; set; }

        public OrganizationMobileDetail? Detail { get; set; }
    }

    public class OrganizationMobileDetail
    {
        [Key]
        [MaxLength(100)]
        public string? TaxCode { get; set; }

        [MaxLength(500)]
        public string? LegalName { get; set; }

        [MaxLength(1000)]
        public string? PrimaryImagePath { get; set; }

        [MaxLength(500)]
        public string? Type { get; set; }

        [MaxLength(1000)]
        public string? Address { get; set; }

        [MaxLength(4000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? MainFunction { get; set; }

        public DateTime? FoundationDate { get; set; }

        [MaxLength(200)]
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

        public List<FeatureCard>? Neighbors { get; set; } = [];

        public NearestCarPark? NearestCarPark { get; set; }

        public List<OwnedPoi>? OwnedPoi { get; set; } = [];

        public List<Offer>? Offers { get; set; } = [];

        public List<PublicEventCard>? Events { get; set; } = [];

        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }

    public class OwnedPoi
    {
        [Key]
        [MaxLength(200)]
        public string? Identifier { get; set; }

        [MaxLength(500)]
        public string? OfficialName { get; set; }

        [MaxLength(1000)]
        public string? ImagePath { get; set; }

        [MaxLength(200)]
        public string? Category { get; set; }
    }
}
