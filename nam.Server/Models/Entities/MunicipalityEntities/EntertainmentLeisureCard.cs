using System.ComponentModel.DataAnnotations;

namespace nam.Server.Models.Entities.MunicipalityEntities
{
    public class EntertainmentLeisureCard
    {
        [Key]
        public Guid? EntityId { get; set; }

        public string? EntityName { get; set; }

        public string? ImagePath { get; set; }

        public string? BadgeText { get; set; }

        public string? Address { get; set; }

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
        public string OfficialName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string PrimaryImagePath { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string FullAddress { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string Description { get; set; } = string.Empty;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public ICollection<string> Gallery { get; set; } = [];

        public ICollection<string> VirtualTours { get; set; } = [];

        public ICollection<FeatureCardRelationship<EntertainmentLeisureDetail>> Neighbors { get; set; } = [];

        public NearestCarPark? NearestCarPark { get; set; }

        public ICollection<AssociatedService> AssociatedServices { get; set; } = [];

        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }
    }
}
