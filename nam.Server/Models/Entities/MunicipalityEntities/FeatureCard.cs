using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace nam.Server.Models.Entities.MunicipalityEntities
{
    public class FeatureCard
    {
        [Key]
        public Guid EntityId { get; set; }

        [MaxLength(255)]
        public string? Title { get; set; }

        public MobileCategory Category { get; set; }

        [MaxLength(500)]
        public string? ImagePath { get; set; }

        public string? ExtraInfo { get; set; }

        [JsonIgnore]
        public List<FeatureCardRelationship<ArtCultureNatureDetail>> ArtCultureRelations { get; set; } = [];

        [JsonIgnore]
        public List<FeatureCardRelationship<EntertainmentLeisureDetail>> EntertainmentLeisureRelations { get; set; } = [];

        [JsonIgnore]
        public List<FeatureCardRelationship<MunicipalityHomeInfo>> MunicipalityHomeInfoEventsRelations { get; set; } = [];

        //[JsonIgnore]
        //public List<FeatureCardRelationship<MunicipalityHomeInfo>> MunicipalityHomeInfoArticlesAndPathsRelations { get; set; } = [];

        //[JsonIgnore]
        //public List<FeatureCardRelationship<OrganizationMobileDetail>> OrganizationMobileDetailRelations { get; set; } = [];

        //[JsonIgnore]
        //public List<FeatureCardRelationship<PublicEventMobileDetail>> PublicEventMobileDetailRelations { get; set; } = [];
    }

    public class FeatureCardRelationship<TEntity>
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public FeatureCard? FeatureCard { get; set; }

        [JsonIgnore]
        public TEntity? RelatedEntity { get; set; }
    }
}
