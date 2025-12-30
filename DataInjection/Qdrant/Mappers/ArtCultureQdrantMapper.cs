using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Qdrant.Mappers
{
    public class ArtCultureQdrantMapper : IDtoMapper<ArtCultureNatureCard, POIEntity>
    {
        POIEntity IDtoMapper<ArtCultureNatureCard, POIEntity>.MapToEntity(ArtCultureNatureCard dto)
        {
            return new POIEntity
            {
                apiEndpoint = "/api/art-culture/card",
                EntityId = dto.EntityId.ToString(),
                //city = dto.Detail.MunicipalityData.Name,
                //lat = dto.Detail.Latitude,
                //lon = dto.Detail.Longitude
            };
        }
    }
}
