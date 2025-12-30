using DataInjection.Interfaces;
using DataInjection.Providers;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Mappers
{
    public class ArtCultureQdrantCollector(IEmbeddingGenerator<string, Embedding<float>> embedder, IConfiguration configuration, IFetcher fetcher) : POIVectorEntityCollector<ArtCultureNatureCard>(embedder)
    {

        private WebServerProvider<List<ArtCultureNatureCard>, List<POIEntity>> _cardProvider;

        public override AbstractProvider<List<ArtCultureNatureCard>, List<POIEntity>> GetProvider()
        {
            return _cardProvider ?? new(
                configuration,
               fetcher,
               new ArtCultureQdrantMapper(),
               "/api/art-culture/card-list",
               new Dictionary<string, string?> { { "municipality", "" } }
            );
        }
    }
}
