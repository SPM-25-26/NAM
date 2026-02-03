

using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class MapMarker
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(500)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string ImagePath { get; set; } = string.Empty;

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [MaxLength(100)]
        public string Typology { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Address { get; set; } = string.Empty;
    }
}
