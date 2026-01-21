

using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class TemporaryClosure
    {
        [MaxLength(1000)]
        [Embeddable]
        [Required]
        public string ReasonForClosure { get; set; }
        [Embeddable]
        public DateTime? Opens { get; set; }
        [Embeddable]
        public DateTime? Closes { get; set; }
        [MaxLength(1000)]
        [Embeddable]
        public string? Description { get; set; }
        [Embeddable]
        public TimeInterval? TimeInterval { get; set; }
        [Embeddable]
        public DayOfWeek? Day { get; set; }
    }
}
