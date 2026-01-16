using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Mappers;
using Domain.Entities.MunicipalityEntities;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Collectors
{
    public class NatureQdrantCollector(Serilog.ILogger logger, IEmbeddingGenerator<string, Embedding<float>> embedder, IConfiguration configuration, IFetcher fetcher) : POIVectorEntityCollector<Nature>(logger, embedder, configuration, fetcher)
    {
        public override string getEndpoint()
        {
            return "/api/nature/full-card-list";
        }

        public override IDtoMapper<Nature, POIEntity> getMapper()
        {
            return new NatureQdrantMapper();
        }

        public override Dictionary<string, string> getQuery()
        {
            return [];
        }
    }
}
