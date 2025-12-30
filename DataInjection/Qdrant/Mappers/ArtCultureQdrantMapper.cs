using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Qdrant.Mappers
{
    public class ArtCultureQdrantMapper : IDtoMapper<List<ArtCultureNatureCard>, List<POIEntity>>
    {
        public List<POIEntity> MapToEntity(List<ArtCultureNatureCard> dto)
        {
            return dto.Select(e =>
            new POIEntity
            {
                apiEndpoint = "/api/art-culture/card",
                EntityId = e.EntityId.ToString(),
                //city = e.Detail.MunicipalityData.Name,
                //lat = e.Detail.Latitude,
                //lon = e.Detail.Longitude
            }).ToList();
        }
    }
}
