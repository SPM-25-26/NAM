using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Embedders;
using DataInjection.Qdrant.Mappers;
using Qdrant.Client.Grpc;
using System.Text.Json;

namespace DataInjection.Qdrant.Stringers
{
    public abstract class AbstractQdrantMapper<TEntity>(IEntityCollector<TEntity> collector, IEmbedder embedder) : IQdrantPayloadCollector
    {
        public abstract QdrantPayload MapToQdrantPayload(TEntity entity);

        public async Task<List<PointStruct>> GetEntities(string municipality)
        {
            var entities = await collector.GetEntities(municipality);
            var tasks = entities.Select(async e =>
            {
                var entityString = e?.ToString() ?? string.Empty;
                var vector = await embedder.GetEmbeddingAsync(entityString);
                var payload = MapToQdrantPayload(e);
                return new PointStruct
                {
                    Id = Guid.NewGuid(),
                    Vectors = vector,
                    Payload =
                    {
                        ["apiEndpoint"] = payload.apiEndpoint,
                        ["apiQuery"] = JsonSerializer.Serialize(payload.apiQuery),
                        ["lat"] = payload.Location.lat,
                        ["lon"] = payload.Location.lon
                    }
                };
            });
            return await Task.WhenAll(tasks).ConfigureAwait(false) is var results ? results.ToList() : [];
        }
    }
}
