using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Mappers;
using Domain.Entities.MunicipalityEntities;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Collectors
{
    public class ArtCultureQdrantCollector(Serilog.ILogger logger, IEmbeddingGenerator<string, Embedding<float>> embedder, IConfiguration configuration, IFetcher fetcher) : POIVectorEntityCollector<ArtCultureNatureCard>(logger, embedder, configuration, fetcher)
    {

        public override string getEndpoint()
        {
            return "/api/art-culture/full-card-list";
        }

        public override IDtoMapper<ArtCultureNatureCard, POIEntity> getMapper()
        {
            return new ArtCultureQdrantMapper();
        }

        public override Dictionary<string, string> getQuery()
        {
            return [];
        }
    }
}
