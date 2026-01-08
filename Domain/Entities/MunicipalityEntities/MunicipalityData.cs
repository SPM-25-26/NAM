using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class MunicipalityForLocalStorageSetting
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        [Embeddable]
        public string? Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? LogoPath { get; set; } = string.Empty;
    }
}
