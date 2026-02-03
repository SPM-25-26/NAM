using Domain.Attributes;
using Domain.Entities.MunicipalityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInjection.SQL.DTOs
{
    public class TypicalProductDto
    {
        public string Identifier { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? CityName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public EntityStatus? Status { get; set; }
        public TypicalProductCategory? Type { get; set; }
        public QualityCertification? Certification { get; set; }
    }
}
