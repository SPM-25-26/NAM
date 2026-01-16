using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Mappers;
using Domain.Entities.MunicipalityEntities;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Collectors
{
    public class PublicEventQdrantCollector(Serilog.ILogger logger, IEmbeddingGenerator<string, Embedding<float>> embedder, IConfiguration configuration, IFetcher fetcher) : POIVectorEntityCollector<PublicEventCard>(logger, embedder, configuration, fetcher)
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
