

using Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.MunicipalityEntities
{
    public class AdmissionType
    {
        [Embeddable]
        public AdmissionTypeName? Name { get; set; }
        [MaxLength(1000)]
        [Embeddable]
        public string? Description { get; set; }
    }
}
