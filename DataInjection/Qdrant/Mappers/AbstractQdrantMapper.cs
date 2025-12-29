using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using Microsoft.Extensions.AI;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DataInjection.Qdrant.Mappers
{
    public abstract class AbstractQdrantMapper<TEntity>(IEntityCollector<TEntity> collector, IEmbeddingGenerator<string, Embedding<float>> embedder, int outputDimensionality) : IQdrantPayloadCollector
    {
        readonly ISerializer serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        public abstract QdrantFormat MapToQdrantPayload(TEntity entity);

        async Task<List<QdrantFormat>> IEntityCollector<QdrantFormat>.GetEntities(string municipality)
        {
            var entities = await collector.GetEntities(municipality);
            var tasks = entities.Select(async e =>
            {
                var entityString = serializer.Serialize(e);
                var embeddingResult = await embedder.GenerateAsync(entityString);
                var vector = embeddingResult.Vector;
                var payload = MapToQdrantPayload(e);
                return new QdrantFormat
                {
                    Id = Guid.NewGuid(),
                    Vector = vector,
                    apiEndpoint = payload.apiEndpoint,
                    EntityId = payload.EntityId,
                    city = payload.city,
                    lat = payload.lat,
                    lon = payload.lon
                };
            });
            return await Task.WhenAll(tasks).ConfigureAwait(false) is var results ? results.ToList() : [];
        }
    }
}
