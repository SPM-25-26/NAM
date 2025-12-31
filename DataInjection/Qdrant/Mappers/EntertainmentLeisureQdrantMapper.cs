using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Qdrant.Mappers
{
    internal class EntertainmentLeisureQdrantMapper : IDtoMapper<EntertainmentLeisureCard, POIEntity>
    {
        public POIEntity MapToEntity(EntertainmentLeisureCard dto)
        {
            return new POIEntity
            {
                apiEndpoint = "/api/art-culture/card",
                EntityId = dto.EntityId.ToString(),
                city = dto.Detail.MunicipalityData.Name,
                lat = dto.Detail.Latitude,
                lon = dto.Detail.Longitude
            };
        }
    }
}
