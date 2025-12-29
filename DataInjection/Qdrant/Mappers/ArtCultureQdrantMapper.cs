using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Mappers
{
    internal class ArtCultureQdrantMapper(IEntityCollector<ArtCultureNatureCard> collector, IEmbeddingGenerator<string, Embedding<float>> embedder, int outputDimensionality) : AbstractQdrantMapper<ArtCultureNatureCard>(collector, embedder, outputDimensionality)
    {
        public override QdrantFormat MapToQdrantPayload(ArtCultureNatureCard entity)
        {
            return new QdrantFormat
            {
                apiEndpoint = "/api/art-culture/card",
                EntityId = entity.EntityId.ToString(),
                city = entity.Detail.MunicipalityData.Name,
                lon = entity.Detail.Longitude,
                lat = entity.Detail.Latitude,
            };
        }
    }
}
