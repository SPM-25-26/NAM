

using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class Booking
    {
        [Embeddable]
        public TimeInterval? TimeIntervalDto { get; set; }
        [Embeddable]
        public BookingType Name { get; set; }
        [MaxLength(1000)]
        [Embeddable]
        public string? Description { get; set; }
    }
}
