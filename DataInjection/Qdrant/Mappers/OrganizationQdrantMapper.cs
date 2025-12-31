using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Qdrant.Mappers
{
    public class OrganizationQdrantMapper : IDtoMapper<OrganizationCard, POIEntity>
    {
        public POIEntity MapToEntity(OrganizationCard dto)
        {
            return new POIEntity
            {
                apiEndpoint = "/api/organizations/card",
                EntityId = dto.TaxCode,
                city = dto.Detail.MunicipalityData.Name,
                lat = dto.Detail.Latitude,
                lon = dto.Detail.Longitude
            };
        }
    }
}
