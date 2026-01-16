using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Qdrant.Mappers
{
    public class NatureQdrantMapper : IDtoMapper<Nature, POIEntity>
    {
        public POIEntity MapToEntity(Nature dto)
        {
            return new POIEntity
            {
                apiEndpoint = "/api/nature/card",
                EntityId = dto.EntityId.ToString(),
                city = dto.Detail.MunicipalityData.Name,
                lat = dto.Detail.Latitude,
                lon = dto.Detail.Longitude
            };
        }
    }
}
