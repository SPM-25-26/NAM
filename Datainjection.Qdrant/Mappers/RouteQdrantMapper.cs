using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;

namespace Datainjection.Qdrant.Mappers
{
    internal class RouteQdrantMapper : IDtoMapper<RouteCard, POIEntity>
    {
        public POIEntity MapToEntity(RouteCard dto)
        {
            return new POIEntity
            {
                apiEndpoint = "/api/services/card",
                EntityId = dto.EntityId.ToString(),
                city = dto.Detail.MunicipalityData.Name,
                lat = 0,
                lon = 0
            };
        }
    }
}
