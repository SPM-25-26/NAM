using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Mappers;
using Domain.Entities.MunicipalityEntities;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Collectors
{
    public class ArticleQdrantCollector(Serilog.ILogger logger, IEmbeddingGenerator<string, Embedding<float>> embedder, IConfiguration configuration, IFetcher fetcher) : POIVectorEntityCollector<ArticleCard>(logger, embedder, configuration, fetcher)
    {
        public override string getEndpoint()
        {
            return "/api/article/full-card-list";
        }

        public override IDtoMapper<ArticleCard, POIEntity> getMapper()
        {
            return new ArticleQdrantMapper();
        }

        public override Dictionary<string, string> getQuery()
        {
            return [];
        }
    }
}
