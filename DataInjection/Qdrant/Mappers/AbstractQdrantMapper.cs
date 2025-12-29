using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using Microsoft.Extensions.AI;

namespace DataInjection.Qdrant.Mappers
{
    public abstract class AbstractQdrantMapper<TEntity>(IEntityCollector<TEntity> collector, IEmbeddingGenerator<string, Embedding<float>> embedder, int outputDimensionality) : IQdrantPayloadCollector
    {
        public abstract QdrantPayload MapToQdrantPayload(TEntity entity);

        async Task<List<QdrantFormat>> IEntityCollector<QdrantFormat>.GetEntities(string municipality)
        {
            var entities = await collector.GetEntities(municipality);
            var tasks = entities.Select(async e =>
            {
                var entityString = e?.ToString() ?? string.Empty;
                var embeddingResult = await embedder.GenerateAsync(entityString);
                var vector = embeddingResult.Vector;
                var payload = MapToQdrantPayload(e);
                return new QdrantFormat
                {
                    Id = Guid.NewGuid(),
                    Vector = vector,
                    number = outputDimensionality,
                    //Payload =
                    //{
                    //    ["apiEndpoint"] = payload.apiEndpoint,
                    //    ["apiQuery"] = JsonSerializer.Serialize(payload.apiQuery),
                    //    ["lat"] = payload.Location.lat,
                    //    ["lon"] = payload.Location.lon
                    //}
                };
            });
            return await Task.WhenAll(tasks).ConfigureAwait(false) is var results ? results.ToList() : [];
        }
    }
}
