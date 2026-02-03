using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Mappers;
using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Collectors
{
    public class ArticleQdrantCollector(Serilog.ILogger logger, IEmbeddingGenerator<string, Embedding<float>> embedder, IUnitOfWork unitOfWork) : POIVectorEntityCollector<ArticleCard, ArticleDetail, Guid>(logger, embedder, unitOfWork)
    {
        public override IDtoMapper<ArticleCard, POIEntity> getMapper()
        {
            return new ArticleQdrantMapper();
        }

        public override IMunicipalityEntityRepository<ArticleCard, ArticleDetail, Guid> GetRepository()
        {
            return unitOfWork.Article;
        }
    }
}
