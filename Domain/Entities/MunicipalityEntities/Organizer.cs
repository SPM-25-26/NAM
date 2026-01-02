using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class Organizer
    {
        [Key]
        [Embeddable]
        public string? TaxCode { get; set; }

        [Embeddable]
        public string? LegalName { get; set; }

        public string? Website { get; set; }
    }
}
