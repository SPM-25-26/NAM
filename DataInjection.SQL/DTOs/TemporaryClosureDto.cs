

namespace DataInjection.SQL.DTOs
{
    public class TemporaryClosureDto
    {
        public string ReasonForClosure { get; set; }
        public TimeOnly? Opens { get; set; }
        public TimeOnly? Closes { get; set; }
        public string? Description { get; set; }
        public TimeIntervalDto? TimeInterval { get; set; }
        public Domain.Entities.MunicipalityEntities.DayOfWeek? Day { get; set; }
    }
}
