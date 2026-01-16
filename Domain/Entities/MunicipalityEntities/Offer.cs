using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }

        [Embeddable]
        public string? Description { get; set; }

        [Embeddable]
        public double PriceSpecificationCurrencyValue { get; set; }

        [Embeddable]
        public Currency? Currency { get; set; }

        [Embeddable]
        public string? ValidityDescription { get; set; }

        public DateTime? ValidityStartDate { get; set; }

        public DateTime? ValidityEndDate { get; set; }

        [Embeddable]
        public string? UserTypeName { get; set; }

        [Embeddable]
        public string? UserTypeDescription { get; set; }

        [Embeddable]
        public string? TicketDescription { get; set; }

        [Embeddable]
        public virtual PublicEventMobileDetail? PublicEventMobileDetail { get; set; }
    }
}
