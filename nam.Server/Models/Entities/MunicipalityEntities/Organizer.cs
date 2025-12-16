using System.ComponentModel.DataAnnotations;

namespace nam.Server.Models.Entities.MunicipalityEntities
{
    public class Organizer
    {
        [Key]
        public string? TaxCode { get; set; }

        public string? LegalName { get; set; }

        public string? Website { get; set; }
    }
}
