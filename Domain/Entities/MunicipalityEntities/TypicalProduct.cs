

using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class TypicalProduct
    {
        [Key]
        public string Identifier { get; set; }
        [MaxLength(255)]
        [Embeddable]
        public string? Name { get; set; }
        [MaxLength(1000)]
        [Embeddable]
        public string? Description { get; set; }
        [MaxLength(500)]
        [Embeddable]
        public string? Address { get; set; }
        [MaxLength(255)]
        [Embeddable]
        public string? CityName { get; set; }
        [Embeddable]
        public DateTime? CreatedAt { get; set; }
        [Embeddable]
        public EntityStatus? Status { get; set; }
        [Embeddable]
        public TypicalProductCategory? Type { get; set; }
        [Embeddable]
        public QualityCertification? Certification { get; set; }
    }
}
