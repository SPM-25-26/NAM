using System.ComponentModel.DataAnnotations;

namespace nam.Server.Models.Entities.MunicipalityEntities
{
    public class PublicEventCard
    {
        [Key]
        public Guid EntityId { get; set; }

        [Required]
        [MaxLength(500)]
        public string EntityName { get; set; } = default!;

        [Required]
        [MaxLength(1000)]
        public string ImagePath { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        public string BadgeText { get; set; } = default!;

        [Required]
        [MaxLength(1000)]
        public string Address { get; set; } = default!;

        public Guid? MunicipalityDataId { get; set; }

        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }

        [Required]
        [MaxLength(255)]
        public string Date { get; set; } = default!;
    }
}
