using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Mappers;
using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Collectors
{
    public class OrganizationQdrantCollector(Serilog.ILogger logger, IEmbeddingGenerator<string, Embedding<float>> embedder, IUnitOfWork unitOfWork) : POIVectorEntityCollector<OrganizationCard, OrganizationMobileDetail, string>(logger, embedder, unitOfWork)
    {
        public override IDtoMapper<OrganizationCard, POIEntity> getMapper()
        {
            return new OrganizationQdrantMapper();
        }

        public override IMunicipalityEntityRepository<OrganizationCard, OrganizationMobileDetail, string> GetRepository()
        {
            return unitOfWork.Organization;
        }
    }
}
