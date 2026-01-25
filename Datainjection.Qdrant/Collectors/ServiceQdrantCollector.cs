using Datainjection.Qdrant.Mappers;
using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Collectors;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.AI;

namespace Datainjection.Qdrant.Collectors
{
    internal class ServiceQdrantCollector(
        Serilog.ILogger logger,
        IEmbeddingGenerator<string, Embedding<float>> embedder,
        IUnitOfWork unitOfWork
        ) : POIVectorEntityCollector<ServiceCard, ServiceDetail, Guid>(logger, embedder, unitOfWork)
    {
        public override IDtoMapper<ServiceCard, POIEntity> getMapper()
        {
            return new ServiceQdrantMapper();
        }

        public override IMunicipalityEntityRepository<ServiceCard, ServiceDetail, Guid> GetRepository()
        {
            return unitOfWork.Service;
        }
    }
}
