using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using Microsoft.Extensions.AI;
using System.Security.Cryptography;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DataInjection.Qdrant.Mappers
{
    public abstract class POIVectorEntityCollector<TEntity>(IEmbeddingGenerator<string, Embedding<float>> embedder, IConfiguration configuration, IFetcher fetcher) : IEntityCollector<POIEntity>
    {
        private static readonly Guid MyNamespaceId = Guid.Parse("d34b9678-7554-4634-8f7a-8534f37803a5");
        private readonly ISerializer serializer = new SerializerBuilder()
            .EnsureRoundtrip()
            .WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

        public abstract string getEndpoint();
        public abstract Dictionary<string, string> getQuery();

        public abstract IDtoMapper<TEntity, POIEntity> getMapper();

        private Guid HandleString(string text)
        {
            using MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            Guid result = new Guid(hash);
            return result;
        }

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
            var mapper = getMapper();
            var query = getQuery();
            query["municipality"] = municipality;
            var entities = await fetcher.Fetch<List<TEntity>>(configuration["SERVER_HTTPS"], getEndpoint(), query);

            var poiEntities = new List<POIEntity>();
            foreach (var e in entities)
            {
                var entityString = serializer.Serialize(e);
                var chunks = ChunkWithOverlap(entityString);

                int chunkCounter = 0;

                foreach (var chunk in chunks)
                {
                    chunkCounter++;

                    var embeddingResult = await embedder.GenerateAsync(chunk);
                    var vector = embeddingResult.Vector;
                    var poiEntity = mapper.MapToEntity(e);
                    poiEntity.Id = Guid.TryParse(poiEntity.EntityId, out var id)
                        ? id
                        : HandleString(poiEntity.EntityId);
                    poiEntity.Vector = vector;
                    poiEntity.chunkPart = chunkCounter;

                    poiEntities.Add(poiEntity);
                }
            }
            return poiEntities;
        }
    }
}
