using System.ComponentModel.DataAnnotations;

namespace nam.Server.Models.Entities.MunicipalityEntities
{
    public class MunicipalityCard
    {
        [Key]
        [MaxLength(255)]
        public string? LegalName { get; set; }

        [MaxLength(1000)]
        public string? ImagePath { get; set; }

        public MunicipalityHomeInfo? Detail { get; set; }
    }
}
