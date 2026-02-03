using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;

namespace Datainjection.Qdrant.Mappers
{
    internal class ShoppingQdrantMapper : IDtoMapper<ShoppingCard, POIEntity>
    {
        public POIEntity MapToEntity(ShoppingCard dto)
        {
            return new POIEntity
            {
                apiEndpoint = "/api/shopping/card",
                EntityId = dto.EntityId.ToString(),
                city = dto.Detail.MunicipalityData.Name,
                lat = dto.Detail.Latitude,
                lon = dto.Detail.Longitude
            };
        }
    }
}
