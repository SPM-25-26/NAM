using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;

namespace Datainjection.Qdrant.Mappers
{
    internal class SleepQdrantMapper : IDtoMapper<SleepCard, POIEntity>
    {
        public POIEntity MapToEntity(SleepCard dto)
        {
            return new POIEntity
            {
                apiEndpoint = "/api/sleep/card",
                EntityId = dto.EntityId.ToString(),
                city = dto.Detail.MunicipalityData.Name,
                lat = dto.Detail.Latitude,
                lon = dto.Detail.Longitude
            };
        }
    }
}
