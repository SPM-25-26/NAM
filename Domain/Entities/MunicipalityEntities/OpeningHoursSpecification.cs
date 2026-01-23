using Domain.Attributes;
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities.MunicipalityEntities
{
    public class OpeningHoursSpecification
    {
        [Embeddable]
        public TimeOnly Opens { get; set; }
        [Embeddable]
        public TimeOnly Closes { get; set; }
        [MaxLength(1000)]
        [Embeddable]    
        public string? Description { get; set; }
        [Embeddable]
        public AdmissionType AdmissionType { get; set; }
        [Embeddable]
        public TimeInterval TimeInterval { get; set; }
        [Embeddable]
        public DayOfWeek? Day { get; set; }
    }
}
