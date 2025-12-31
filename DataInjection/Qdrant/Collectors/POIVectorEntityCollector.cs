using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using Microsoft.Extensions.AI;
using System.Security.Cryptography;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DataInjection.Qdrant.Collectors
{
    public abstract class POIVectorEntityCollector<TEntity>(IEmbeddingGenerator<string, Embedding<float>> embedder, IConfiguration configuration, IFetcher fetcher) : IEntityCollector<POIEntity>
    {

        private readonly ISerializer serializer = new SerializerBuilder()
            .EnsureRoundtrip()
            .WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

        public abstract string getEndpoint();
        public abstract Dictionary<string, string> getQuery();

        public abstract IDtoMapper<TEntity, POIEntity> getMapper();

        private static List<List<T>> SplitList<T>(List<T> source, int batchSize)
        {
            var result = new List<List<T>>();
            for (int i = 0; i < source.Count; i += batchSize)
            {
                result.Add(source.GetRange(i, Math.Min(batchSize, source.Count - i)));
            }
            return result;
        }

        private Guid HandleString(string text)
        {
            using MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            Guid result = new Guid(hash);
            return result;
        }

        // TODO : Use API to get token count instead of word count
        static List<string> ChunkWithOverlap(string input, int maxTokens = 500, double overlapRate = 0.15)
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
            var allMetadata = new List<ChunkMetadata>();

            foreach (var e in entities)
            {
                var entityString = serializer.Serialize(e);
                var chunks = ChunkWithOverlap(entityString);

                for (int i = 0; i < chunks.Count; i++)
                {
                    allMetadata.Add(new ChunkMetadata(e, chunks[i], i + 1));
                }
            }

            // Batch Embedding
            var allEmbeddings = new List<Embedding<float>>();
            var allTexts = allMetadata.Select(m => m.Text).ToList();
            var batches = SplitList(allTexts, 5);
            Console.WriteLine($"Total texts to embed: {allTexts.Count} in {batches.Count} batches.");
            foreach (var batch in batches)
            {
                Console.WriteLine($"Generating embeddings for batch of size {batch.Count}...");
                var embed = await embedder.GenerateAsync(batch);
                Console.WriteLine($"Embeddings generated, waiting 60 seconds.");

                await Task.Delay(1000 * 61);

                foreach (var e in embed)
                {
                    allEmbeddings.Add(e);
                }
            }
            Console.WriteLine($"Embeddings done");


            //var allEmbeddings = await embedder.GenerateAsync(allTexts);

            for (int i = 0; i < allMetadata.Count; i++)
            {
                var metadata = allMetadata[i];
                var poiEntity = mapper.MapToEntity(metadata.Entity);

                poiEntity.Id = Guid.TryParse(poiEntity.EntityId, out var id)
                    ? id
                    : HandleString(poiEntity.EntityId);

                // Assegna il vettore corrispondente dall'indice globale
                poiEntity.Vector = allEmbeddings[i].Vector;
                poiEntity.chunkPart = metadata.ChunkIndex;

                poiEntities.Add(poiEntity);
            }

            return poiEntities;
        }
        private record ChunkMetadata(TEntity Entity, string Text, int ChunkIndex);
    }
}
