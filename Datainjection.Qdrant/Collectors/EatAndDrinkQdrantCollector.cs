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
    internal class EatAndDrinkQdrantCollector(
        Serilog.ILogger logger,
        IEmbeddingGenerator<string, Embedding<float>> embedder,
        IUnitOfWork unitOfWork
        ) : POIVectorEntityCollector<EatAndDrinkCard, EatAndDrinkDetail, Guid>(logger, embedder, unitOfWork)

    {
        public override IDtoMapper<EatAndDrinkCard, POIEntity> getMapper()
        {
            return new EatAndDrinkQdrantMapper();
        }

        public override IMunicipalityEntityRepository<EatAndDrinkCard, EatAndDrinkDetail, Guid> GetRepository()
        {
            return unitOfWork.EatAndDrink;
        }
    }
}
