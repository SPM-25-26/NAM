using System.ComponentModel.DataAnnotations;

namespace nam.Server.Models.Entities.MunicipalityEntities
{
    public class MunicipalityForLocalStorageSetting
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string LogoPath { get; set; } = string.Empty;
    }
}
