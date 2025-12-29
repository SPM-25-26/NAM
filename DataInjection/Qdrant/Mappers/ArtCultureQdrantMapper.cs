using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using Domain.Entities.MunicipalityEntities;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Mappers
{
    internal class ArtCultureQdrantMapper(IEntityCollector<ArtCultureNatureCard> collector, IEmbeddingGenerator<string, Embedding<float>> embedder, int outputDimensionality) : AbstractQdrantMapper<ArtCultureNatureCard>(collector, embedder, outputDimensionality)
    {
        public override QdrantPayload MapToQdrantPayload(ArtCultureNatureCard entity)
        {
            return new QdrantPayload
            {
                apiEndpoint = "/api/art-culture/card",
                apiQuery = new Dictionary<string, string>
                {
                    { "id", entity.ToString() }
                },
                Location = entity.Detail is not null
                    ? new Location
                    {
                        lat = entity.Detail.Latitude,
                        lon = entity.Detail.Longitude
                    }
                    : new Location { lat = 0, lon = 0 }
            };
        }
    }
}
