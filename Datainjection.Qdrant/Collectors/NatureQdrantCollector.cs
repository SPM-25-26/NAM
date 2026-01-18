using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Mappers;
using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Collectors
{
    public class NatureQdrantCollector(Serilog.ILogger logger, IEmbeddingGenerator<string, Embedding<float>> embedder, IUnitOfWork unitOfWork) : POIVectorEntityCollector<Nature, ArtCultureNatureDetail, Guid>(logger, embedder, unitOfWork)
    {
        public override IDtoMapper<Nature, POIEntity> getMapper()
        {
            return new NatureQdrantMapper();
        }

        public override IMunicipalityEntityRepository<Nature, ArtCultureNatureDetail, Guid> GetRepository()
        {
            return unitOfWork.Nature;
        }
    }
}
