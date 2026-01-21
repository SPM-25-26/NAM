using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class Point
    {
        [Key]
        public string Address { get; set; }

        [Embeddable]
        public double Latitude { get; set; }

        [Embeddable]
        public double Longitude { get; set; }
    }
}
