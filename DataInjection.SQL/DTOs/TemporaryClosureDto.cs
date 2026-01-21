

namespace DataInjection.SQL.DTOs
{
    public class TemporaryClosureDto
    {
        public string ReasonForClosure { get; set; }
        public DateTime? Opens { get; set; }
        public DateTime? Closes { get; set; }
        public string? Description { get; set; }
        public TimeIntervalDto? TimeInterval { get; set; }
        public Domain.Entities.MunicipalityEntities.DayOfWeek? Day { get; set; }
    }
}
