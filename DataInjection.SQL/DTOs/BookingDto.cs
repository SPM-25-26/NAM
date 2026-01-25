using Domain.Entities.MunicipalityEntities;

namespace DataInjection.SQL.DTOs
{
    public class BookingDto
    {
        public TimeIntervalDto? TimeIntervalDto { get; set; }
        public BookingType Name { get; set; }

        public string? Description { get; set; }


}
}
