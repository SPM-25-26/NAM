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
    internal class RouteQdrantCollector(
        Serilog.ILogger logger,
        IEmbeddingGenerator<string, Embedding<float>> embedder,
        IUnitOfWork unitOfWork
        ) : POIVectorEntityCollector<RouteCard, RouteDetail, Guid>(logger, embedder, unitOfWork)
    {
        public override IDtoMapper<RouteCard, POIEntity> getMapper()
        {
            return new RouteQdrantMapper();
        }

        public override IMunicipalityEntityRepository<RouteCard, RouteDetail, Guid> GetRepository()
        {
            return unitOfWork.Route;
        }
    }
}
