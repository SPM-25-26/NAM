using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Qdrant.Mappers
{
    public class PublicEventQdrantMapper : IDtoMapper<PublicEventCard, POIEntity>
    {
        public POIEntity MapToEntity(PublicEventCard dto)
        {
            return new POIEntity
            {
                apiEndpoint = "/api/public-event/card",
                EntityId = dto.EntityId.ToString(),
                city = dto.Detail.MunicipalityData.Name,
                lat = dto.Detail.Latitude,
                lon = dto.Detail.Longitude
            };
        }
    }
}
