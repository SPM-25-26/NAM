using Domain.Entities.MunicipalityEntities;

namespace DataInjection.SQL.DTOs
{
    public class OfferDto
    {
        public string? Description { get; set; }

        public double PriceSpecificationCurrencyValue { get; set; }

        public Currency? Currency { get; set; }

        public string? ValidityDescription { get; set; }

        public DateTime? ValidityStartDate { get; set; }

        public DateTime? ValidityEndDate { get; set; }

        public string? UserTypeName { get; set; }

        public string? UserTypeDescription { get; set; }

        public string? TicketDescription { get; set; }
    }
}
