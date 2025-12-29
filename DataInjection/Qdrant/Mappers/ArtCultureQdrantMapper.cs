using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Embedders;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.Qdrant.Stringers
{
    internal class ArtCultureQdrantMapper(IEntityCollector<ArtCultureNatureCard> collector, IEmbedder embedder) : AbstractQdrantMapper<ArtCultureNatureCard>(collector, embedder)
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
