using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;

namespace Datainjection.Qdrant.Mappers
{
    public class EatAndDrinkQdrantMapper : IDtoMapper<EatAndDrinkCard, POIEntity>
    {
        public POIEntity MapToEntity(EatAndDrinkCard dto)
        {
            return new POIEntity
            {
                apiEndpoint = "/api/eat-and-drink/card",
                EntityId = dto.EntityId.ToString(),
                city = dto.Detail.MunicipalityData.Name,
                lat = dto.Detail.Latitude,
                lon = dto.Detail.Longitude
            };
        }
    }
}
