using DataInjection.Interfaces;
using DataInjection.Providers;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Mappers
{
    public class ArtCultureQdrantCollector(IEmbeddingGenerator<string, Embedding<float>> embedder, IConfiguration configuration, IFetcher fetcher) : POIVectorEntityCollector<ArtCultureNatureCard>(embedder, configuration, fetcher)
    {

        private WebServerProvider<List<ArtCultureNatureCard>, List<POIEntity>> _cardProvider;

        public override string getEndpoint()
        {
            return "/api/art-culture/card-list";
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
