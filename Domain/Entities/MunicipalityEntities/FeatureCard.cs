using Domain.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities.MunicipalityEntities
{
    public class FeatureCard
    {
        [Key]
        public Guid EntityId { get; set; }

        [MaxLength(255)]
        [Embeddable]
        public string? Title { get; set; }

        [Embeddable]
        public MobileCategory Category { get; set; }

        [MaxLength(500)]
        public string? ImagePath { get; set; }

        [Embeddable]
        public string? ExtraInfo { get; set; }

        [JsonIgnore]
        public List<FeatureCardRelationship<ArtCultureNatureDetail>> ArtCultureRelations { get; set; } = [];

        [JsonIgnore]
        public List<FeatureCardRelationship<EntertainmentLeisureDetail>> EntertainmentLeisureRelations { get; set; } = [];

        //[JsonIgnore]
        //public List<FeatureCardRelationship<MunicipalityHomeInfo>> MunicipalityHomeInfoEventsRelations { get; set; } = [];

        //[JsonIgnore]
        //public List<FeatureCardRelationship<MunicipalityHomeInfo>> MunicipalityHomeInfoArticlesAndPathsRelations { get; set; } = [];

        [JsonIgnore]
        public List<FeatureCardRelationship<OrganizationMobileDetail>> OrganizationMobileDetailRelations { get; set; } = [];

        [JsonIgnore]
        public List<FeatureCardRelationship<PublicEventMobileDetail>> PublicEventMobileDetailRelations { get; set; } = [];

        [JsonIgnore]
        public List<FeatureCardRelationship<RouteDetail>> RouteRelations { get; set; } = [];
        [JsonIgnore]
        public List<FeatureCardRelationship<ServiceDetail>> ServiceRelations { get; set; } = [];
        [JsonIgnore]
        public List<FeatureCardRelationship<ShoppingCardDetail>> ShoppingRelations { get; set; } = [];
        [JsonIgnore]
        public List<FeatureCardRelationship<SleepCardDetail>> SleepRelations { get; set; } = [];
        [JsonIgnore]
        public List<FeatureCardRelationship<EatAndDrinkDetail>> EatAndDrinkRelations { get; set; } = [];
    }

    public class FeatureCardRelationship<TEntity>
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Embeddable]
        public FeatureCard? FeatureCard { get; set; }

        [JsonIgnore]
        public TEntity? RelatedEntity { get; set; }
    }
}
