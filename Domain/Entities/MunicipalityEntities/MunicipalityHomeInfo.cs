using Domain.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities.MunicipalityEntities
{
    public class MunicipalityHomeInfo
    {
        [Key]
        [MaxLength(500)]
        [Embeddable]
        public string? LegalName { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? Name { get; set; }

        [MaxLength(4000)]
        [Embeddable]
        public string? Description { get; set; }

        [Embeddable]
        public MunicipalityHomeContactInfo? Contacts { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [MaxLength(1000)]
        public string? LogoPath { get; set; }

        public ICollection<string>? HomeImages { get; set; } = [];

        [Embeddable]
        public ICollection<FeatureCardRelationship<MunicipalityHomeInfo>> Events { get; set; } = [];

        [Embeddable]
        public ICollection<FeatureCardRelationship<MunicipalityHomeInfo>> ArticlesAndPaths { get; set; } = [];

        [MaxLength(1000)]
        public string? PanoramaPath { get; set; }

        public int? PanoramaWidth { get; set; }

        public ICollection<string> VirtualTourUrls { get; set; } = [];

        [MaxLength(500)]
        [Embeddable]
        public string? NameAndProvince { get; set; }
    }

    public class MunicipalityHomeContactInfo
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(320)]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Telephone { get; set; }

        [MaxLength(1000)]
        public string? Website { get; set; }

        [MaxLength(200)]
        public string? Facebook { get; set; }

        [MaxLength(200)]
        public string? Instagram { get; set; }

        public string? MunicipalityLegalName { get; set; }

        [ForeignKey(("MunicipalityLegalName"))]
        [JsonIgnore]
        public MunicipalityHomeInfo? Municipality { get; set; }
    }
}
