using Domain.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities.MunicipalityEntities
{
    public class StageMobile
    {
        [MaxLength(255)]
        [Embeddable]
        public string? Category { get; set; }

        [Key]
        public Guid PoiIdentifier { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? PoiOfficialName { get; set; }

        [MaxLength(500)]
        [Embeddable]
        public string? PoiImagePath { get; set; }

        [MaxLength(500)]
        [Embeddable]
        public string? PoiImageThumbPath { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? Signposting { get; set; }
        
        [MaxLength(255)]
        [Embeddable]
        public string? SupportService { get; set; }

        [Embeddable]
        public double PoiLatitude { get; set; }

        [Embeddable]
        public double PoiLongitude { get; set; }

        [MaxLength(500)]
        [Embeddable]
        public string? PoiAddress { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? Name { get; set; }

        [Embeddable]
        public int Number { get; set; }

        [MaxLength(1000)]
        [Embeddable]
        public string? Description { get; set; }

        [JsonIgnore]
        public List<StageMobileRelationship<RouteDetail>> RouteRelations { get; set; } = [];
    }

    public class StageMobileRelationship<TEntity>
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Embeddable]
        public StageMobile? StageMobile { get; set; }

        [JsonIgnore]
        public TEntity? RelatedEntity { get; set; }
    }
}
