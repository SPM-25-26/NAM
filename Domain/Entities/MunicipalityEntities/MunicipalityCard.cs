using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class MunicipalityCard
    {
        [Key]
        [MaxLength(255)]
        [Embeddable]
        public string? LegalName { get; set; }

        [MaxLength(1000)]
        public string? ImagePath { get; set; }

        [Embeddable]
        public MunicipalityHomeInfo? Detail { get; set; }
    }
}
