using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Mappers;
using Domain.Entities.MunicipalityEntities;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Collectors
{
    public class PublicEventQdrantCollector(IEmbeddingGenerator<string, Embedding<float>> embedder, IConfiguration configuration, IFetcher fetcher) : POIVectorEntityCollector<PublicEventCard>(embedder, configuration, fetcher)
    {
        public override string getEndpoint()
        {
            return "/api/public-event/full-card-list";
        }

        public override IDtoMapper<PublicEventCard, POIEntity> getMapper()
        {
            return new PublicEventQdrantMapper();
        }

        public override Dictionary<string, string> getQuery()
        {
            return [];
        }
    }
}
