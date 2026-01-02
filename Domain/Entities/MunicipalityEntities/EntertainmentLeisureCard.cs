using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class EntertainmentLeisureCard
    {
        [Key]
        public Guid EntityId { get; set; }

        [Embeddable]
        public string? EntityName { get; set; }

        public string? ImagePath { get; set; }

        [Embeddable]
        public string? BadgeText { get; set; }

        [Embeddable]
        public string? Address { get; set; }

        [Embeddable]
        public EntertainmentLeisureDetail? Detail { get; set; }

    }

    public class EntertainmentLeisureDetail
    {
        [Key]
        [Required]
        [MaxLength(255)]
        public Guid Identifier { get; set; }

        [Required]
        [MaxLength(500)]
        [Embeddable]
        public string OfficialName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string PrimaryImagePath { get; set; } = string.Empty;

        [MaxLength(1000)]
        [Embeddable]
        public string FullAddress { get; set; } = string.Empty;

        [MaxLength(100)]
        [Embeddable]
        public string Category { get; set; } = string.Empty;

        [MaxLength(4000)]
        [Embeddable]
        public string Description { get; set; } = string.Empty;

        [Embeddable]
        public double Latitude { get; set; }

        [Embeddable]
        public double Longitude { get; set; }

        public ICollection<string> Gallery { get; set; } = [];

        public ICollection<string> VirtualTours { get; set; } = [];

        [Embeddable]
        public ICollection<FeatureCardRelationship<EntertainmentLeisureDetail>> Neighbors { get; set; } = [];

        [Embeddable]
        public NearestCarPark? NearestCarPark { get; set; }

        [Embeddable]
        public ICollection<AssociatedService> AssociatedServices { get; set; } = [];

        [Embeddable]
        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }
}
