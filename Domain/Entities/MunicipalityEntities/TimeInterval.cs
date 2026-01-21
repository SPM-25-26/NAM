

using Domain.Attributes;

namespace Domain.Entities.MunicipalityEntities
{
    public class TimeInterval
    {
        [Embeddable]
        public DateTime Date { get; set; }
        [Embeddable]
        public DateTime StartDate { get; set; }
        [Embeddable]
        public DateTime EndDate { get; set; }
    }
}
