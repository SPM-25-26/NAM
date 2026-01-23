using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class MapData
    {
        [Key]
        [Required]
        public string Name { get; set; }

        public ICollection<MapMarker> Marker { get; set; } = [];

        public double CenterLatitude { get; set; }
        public double CenterLongitude { get; set; }

    }
}
