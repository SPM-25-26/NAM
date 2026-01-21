
using Domain.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities.MunicipalityEntities
{
    public class ServiceLocation
    {
        [Key]
        public string Identifier { get; set; }
        [Embeddable]
        public string? OfficialName { get; set; }
        [Embeddable]
        public string? ImagePath { get; set; }
        [Embeddable]
        public string? Category { get; set; }
    }

    public class ServiceLocationRelationship<TEntity>
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Embeddable]
        public ServiceLocation? ServiceLocation { get; set; }

        [JsonIgnore]
        public TEntity? RelatedEntity { get; set; }
    }
}
