using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Mappers;
using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Collectors
{
    public class ArtCultureQdrantCollector(
        Serilog.ILogger logger,
        IEmbeddingGenerator<string, Embedding<float>> embedder,
        IUnitOfWork unitOfWork
        ) : POIVectorEntityCollector<ArtCultureNatureCard, ArtCultureNatureDetail, Guid>(logger, embedder, unitOfWork)
    {
        public override IDtoMapper<ArtCultureNatureCard, POIEntity> getMapper()
        {
            return new ArtCultureQdrantMapper();
        }

        public override IMunicipalityEntityRepository<ArtCultureNatureCard, ArtCultureNatureDetail, Guid> GetRepository()
        {
            return unitOfWork.ArtCulture;
        }
    }
}
