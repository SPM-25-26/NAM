using System.ComponentModel.DataAnnotations;

namespace nam.Server.Models.Entities.MunicipalityEntities
{
    public class Organizer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? TaxCode { get; set; }

        public string? LegalName { get; set; }

        public string? Website { get; set; }
    }
}
