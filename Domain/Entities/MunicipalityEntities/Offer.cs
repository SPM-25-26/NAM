using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }

        public string? Description { get; set; }

        public double PriceSpecificationCurrencyValue { get; set; }

        public Currency? Currency { get; set; }

        public string? ValidityDescription { get; set; }

        public DateTime? ValidityStartDate { get; set; }

        public DateTime? ValidityEndDate { get; set; }

        public string? UserTypeName { get; set; }

        public string? UserTypeDescription { get; set; }

        public string? TicketDescription { get; set; }

        public virtual PublicEventMobileDetail? PublicEventMobileDetail { get; set; }
    }
}
