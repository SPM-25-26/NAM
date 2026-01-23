using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class RouteCard
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
        public RouteDetail? Detail { get; set; }
    }

    public class RouteDetail
    {
        [Key]
        [Required]
        [MaxLength(255)]
        public Guid Identifier { get; set; } = Guid.NewGuid();

        [MaxLength(1000)]
        public string? ImagePath { get; set; }

        [MaxLength(100)]
        [Embeddable]
        public string? Number { get; set; }

        [MaxLength(500)]
        [Embeddable]
        public string? Name { get; set; }

        [MaxLength(4000)]
        [Embeddable]
        public string? Description { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? PathTheme { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? TravellingMethod { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? ShortName { get; set; }
    
        [MaxLength(1000)]
        public string? OrganizationWebsite { get; set; }

        [MaxLength(320)]
        public string? OrganizationEmail { get; set; }

        [MaxLength(500)]
        public string? OrganizationFacebook { get; set; }

        [MaxLength(500)]
        public string? OrganizationInstagram { get; set; }

        [MaxLength(50)]
        public string? OrganizationTelephone { get; set; }

        [MaxLength(1000)]
        public string? Website { get; set; }

        [MaxLength(100)]
        [Embeddable]
        public string? SecurityLevel { get; set; }

        public int NumberOfStages { get; set; }

        public int QuantifiedPathwayPaving { get; set; }

        [MaxLength(100)]
        [Embeddable]
        public string? Duration { get; set; }

        [MaxLength(100)]
        [Embeddable]
        public string? RouteLength { get; set; }

        public ICollection<string> Gallery { get; set; } = [];

        public ICollection<string> VirtualTours { get; set; } = [];

        [Embeddable]
        public List<StageMobileRelationship<RouteDetail>> Stages { get; set; } = [];

        [Embeddable]
        public List<FeatureCardRelationship<RouteDetail>> StagesPoi { get; set; } = [];

        [Embeddable]
        public Point? StartingPoint { get; set; }

        [Embeddable]
        public ICollection<string> BestWhen { get; set; } = [];

        [Embeddable]
        public DateTime? StartDate { get; set; }

        [Embeddable]
        public DateTime? EndDate { get; set; }

        [Embeddable]
        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }
}
