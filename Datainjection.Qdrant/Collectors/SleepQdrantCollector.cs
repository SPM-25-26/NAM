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
    internal class SleepQdrantCollector(
        Serilog.ILogger logger,
        IEmbeddingGenerator<string, Embedding<float>> embedder,
        IUnitOfWork unitOfWork
        ) : POIVectorEntityCollector<SleepCard, SleepCardDetail, Guid>(logger, embedder, unitOfWork)
    {
        public override IDtoMapper<SleepCard, POIEntity> getMapper()
        {
            return new SleepQdrantMapper();
        }

        public override IMunicipalityEntityRepository<SleepCard, SleepCardDetail, Guid> GetRepository()
        {
            return unitOfWork.Sleep;
        }
    }
}
