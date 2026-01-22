using Domain.Entities.MunicipalityEntities;

namespace DataInjection.SQL.DTOs
{
    public class OpeningHoursSpecificationDto
    {
        public TimeOnly Opens { get; set; }
        public TimeOnly Closes { get; set; }
        public string? Description { get; set; }
        public AdmissionTypeDto? AdmissionType { get; set; }
        public TimeIntervalDto? TimeInterval { get; set; }
        public Domain.Entities.MunicipalityEntities.DayOfWeek? Day { get; set; }

    }
}
