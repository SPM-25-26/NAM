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
    internal class ShoppingQdrantCollector(
        Serilog.ILogger logger,
        IEmbeddingGenerator<string, Embedding<float>> embedder,
        IUnitOfWork unitOfWork
        ) : POIVectorEntityCollector<ShoppingCard, ShoppingCardDetail, Guid>(logger, embedder, unitOfWork)
    {
        public override IDtoMapper<ShoppingCard, POIEntity> getMapper()
        {
            return new ShoppingQdrantMapper();
        }

        public override IMunicipalityEntityRepository<ShoppingCard, ShoppingCardDetail, Guid> GetRepository()
        {
            return unitOfWork.Shopping;
        }
    }
}
