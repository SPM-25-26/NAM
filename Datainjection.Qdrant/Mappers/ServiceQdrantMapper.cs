using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;

namespace Datainjection.Qdrant.Mappers
{
    internal class ServiceQdrantMapper : IDtoMapper<ServiceCard, POIEntity>
    {
        public POIEntity MapToEntity(ServiceCard dto)
        {
            return new POIEntity
            {
                apiEndpoint = "/api/services/card",
                EntityId = dto.EntityId.ToString(),
                city = dto.Detail.MunicipalityData.Name,
                lat = dto.Detail.Latitude,
                lon = dto.Detail.Longitude
            };
        }
    }
}
