using Domain.Entities.MunicipalityEntities;


namespace DataInjection.SQL.DTOs
{
    public class AdmissionTypeDto
    {
        public AdmissionTypeName Name { get; set; }
        public string?  Description { get; set; }
    }
}
