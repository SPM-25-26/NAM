using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Qdrant.Mappers
{
    public class ArticleQdrantMapper : IDtoMapper<ArticleCard, POIEntity>
    {
        public POIEntity MapToEntity(ArticleCard dto)
        {
            return new POIEntity
            {
                apiEndpoint = "/api/article/card",
                EntityId = dto.EntityId.ToString(),
                city = dto.Detail.MunicipalityData.Name
            };
        }
    }
}
