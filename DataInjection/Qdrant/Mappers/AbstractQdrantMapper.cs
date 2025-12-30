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

        public abstract POIEntity MapToQdrantPayload(TEntity entity);

        // TODO : Use API to get token count instead of word count
        static List<string> ChunkWithOverlap(string input, int maxTokens = 2024, double overlapRate = 0.15)
        {
            int overlapTokens = (int)(maxTokens * overlapRate);
            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var chunks = new List<string>();
            int start = 0;
            while (start < words.Length)
            {
                int end = Math.Min(start + maxTokens, words.Length);
                var chunk = string.Join(' ', words[start..end]);
                chunks.Add(chunk);
                if (end == words.Length) break;
                start += maxTokens - overlapTokens;
            }
            return chunks;
        }

        async Task<List<POIEntity>> IEntityCollector<POIEntity>.GetEntities(string municipality)
        {
            var entities = await collector.GetEntities(municipality);
            var poiEntities = new List<POIEntity>();

            foreach (var e in entities)
            {
                var payload = MapToQdrantPayload(e);
                var entityString = serializer.Serialize(e);
                var chunks = ChunkWithOverlap(entityString);

                int chunkCounter = 0;

                foreach (var chunk in chunks)
                {
                    chunkCounter++;

                    var embeddingResult = await embedder.GenerateAsync(chunk);
                    var vector = embeddingResult.Vector;

                    poiEntities.Add(new POIEntity
                    {
                        Id = Guid.NewGuid(),
                        Vector = vector,
                        chunkPart = chunkCounter,
                        apiEndpoint = payload.apiEndpoint,
                        EntityId = payload.EntityId,
                        city = payload.city,
                        lat = payload.lat,
                        lon = payload.lon
                    });

                }
            }
            return poiEntities;
        }
    }
}
