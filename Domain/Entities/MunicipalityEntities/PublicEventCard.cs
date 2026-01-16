using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class PublicEventCard
    {
        [Key]
        public Guid EntityId { get; set; }

        [Required]
        [MaxLength(500)]
        [Embeddable]
        public string? EntityName { get; set; } = default!;

        [Required]
        [MaxLength(1000)]
        public string? ImagePath { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        [Embeddable]
        public string? BadgeText { get; set; } = default!;

        [Required]
        [MaxLength(1000)]
        public string? Address { get; set; } = default!;

        [Embeddable]
        public MunicipalityForLocalStorageSetting? MunicipalityData { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Date { get; set; } = default!;

        [Embeddable]
        public PublicEventMobileDetail? Detail { get; set; }
    }
}
