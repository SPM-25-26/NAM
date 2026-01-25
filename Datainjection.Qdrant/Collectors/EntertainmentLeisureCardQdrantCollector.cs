using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Mappers;
using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Collectors
{
    public class EntertainmentLeisureCardQdrantCollector(Serilog.ILogger logger, IEmbeddingGenerator<string, Embedding<float>> embedder, IUnitOfWork unitOfWork) : POIVectorEntityCollector<EntertainmentLeisureCard, EntertainmentLeisureDetail, Guid>(logger, embedder, unitOfWork)
    {

        public override IDtoMapper<EntertainmentLeisureCard, POIEntity> getMapper()
        {
            return new EntertainmentLeisureQdrantMapper();
        }

        public override IMunicipalityEntityRepository<EntertainmentLeisureCard, EntertainmentLeisureDetail, Guid> GetRepository()
        {
            return unitOfWork.EntertainmentLeisure;
        }
    }
}
