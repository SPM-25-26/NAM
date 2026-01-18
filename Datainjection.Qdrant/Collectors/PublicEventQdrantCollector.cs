using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Mappers;
using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Collectors
{
    public class PublicEventQdrantCollector(Serilog.ILogger logger, IEmbeddingGenerator<string, Embedding<float>> embedder, IUnitOfWork unitOfWork) : POIVectorEntityCollector<PublicEventCard, PublicEventMobileDetail, Guid>(logger, embedder, unitOfWork)
    {
        public override IDtoMapper<PublicEventCard, POIEntity> getMapper()
        {
            return new PublicEventQdrantMapper();
        }

        public override IMunicipalityEntityRepository<PublicEventCard, PublicEventMobileDetail, Guid> GetRepository()
        {
            return unitOfWork.PublicEvent;
        }
    }
}
