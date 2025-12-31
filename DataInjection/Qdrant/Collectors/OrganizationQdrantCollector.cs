using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Mappers;
using Domain.Entities.MunicipalityEntities;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Collectors
{
    public class OrganizationQdrantCollector(IEmbeddingGenerator<string, Embedding<float>> embedder, IConfiguration configuration, IFetcher fetcher) : POIVectorEntityCollector<OrganizationCard>(embedder, configuration, fetcher)
    {
        public override string getEndpoint()
        {
            return "/api/organizations/full-card-list";
        }

        public override IDtoMapper<OrganizationCard, POIEntity> getMapper()
        {
            return new OrganizationQdrantMapper();
        }

        public override Dictionary<string, string> getQuery()
        {
            return [];
        }
    }
}
