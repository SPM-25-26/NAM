using DataInjection.Interfaces;
using DataInjection.Providers;
using DataInjection.Qdrant.Data;
using Microsoft.Extensions.AI;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DataInjection.Qdrant.Mappers
{
    public abstract class POIVectorEntityCollector<TEntity>(IEmbeddingGenerator<string, Embedding<float>> embedder) : IEntityCollector<POIEntity>
    {
        private readonly ISerializer serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

        public abstract AbstractProvider<List<TEntity>, List<POIEntity>> GetProvider();


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
            var provider = GetProvider();
            provider.Query["municipality"] = municipality;
            var entities = await provider.GetEntity();

            var poiEntities = new List<POIEntity>();

            foreach (var e in entities)
            {
                var entityString = e.ToString();//serializer.Serialize(e);
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
                        apiEndpoint = e.apiEndpoint,
                        EntityId = e.EntityId,
                        city = e.city,
                        lat = e.lat,
                        lon = e.lon
                    });
                }
            }
            return poiEntities;
        }
    }
}
