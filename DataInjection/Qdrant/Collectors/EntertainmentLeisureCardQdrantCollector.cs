using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Mappers;
using Domain.Entities.MunicipalityEntities;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Collectors
{
    public class EntertainmentLeisureCardQdrantCollector(IEmbeddingGenerator<string, Embedding<float>> embedder, IConfiguration configuration, IFetcher fetcher) : POIVectorEntityCollector<EntertainmentLeisureCard>(embedder, configuration, fetcher)
    {
        public override string getEndpoint()
        {
            return "/api/entertainment-leisure/full-card-list";
        }

        public override IDtoMapper<EntertainmentLeisureCard, POIEntity> getMapper()
        {
            return new EntertainmentLeisureQdrantMapper();
        }

        public override Dictionary<string, string> getQuery()
        {
            return [];
        }
    }
}
