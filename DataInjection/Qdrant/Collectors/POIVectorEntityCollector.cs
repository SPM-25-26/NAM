using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;
using DataInjection.Qdrant.Serializers;
using Microsoft.Extensions.AI;
using Microsoft.ML.Tokenizers;
using Microsoft.SemanticKernel.Text;
using System.Security.Cryptography;
using System.Text;
namespace DataInjection.Qdrant.Collectors
{
    public abstract class POIVectorEntityCollector<TEntity>(Serilog.ILogger logger, IEmbeddingGenerator<string, Embedding<float>> embedder, IConfiguration configuration, IFetcher fetcher) : IEntityCollector<POIEntity>
    {

        // TODO: Use correct tokenizer, this is not the exact one for gemini but is close enough for now
        private readonly TiktokenTokenizer tokenizer = TiktokenTokenizer.CreateForModel("gpt-4o");


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
            byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes(text));
            Guid result = new(hash);
            return result;
        }

        private List<string> ChunkWithOverlap(string input, int maxTokens = 1900, double overlapRate = 0.15)
        {
            var overlapTokens = (int)(maxTokens * overlapRate);
#pragma warning disable SKEXP0050
            TextChunker.TokenCounter geminiCounter = (text) => tokenizer.CountTokens(text);
            var lines = TextChunker.SplitPlainTextLines(input, maxTokensPerLine: 100, tokenCounter: geminiCounter);
            var paragraphs = TextChunker.SplitPlainTextParagraphs(lines, maxTokensPerParagraph: maxTokens, overlapTokens: overlapTokens, tokenCounter: geminiCounter);
#pragma warning restore SKEXP0050
            return paragraphs;
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
                var entityString = e.ToEmbeddingString();
                var chunks = ChunkWithOverlap(entityString);

                for (int i = 0; i < chunks.Count; i++)
                {
                    allMetadata.Add(new ChunkMetadata(e, chunks[i], i + 1));
                }
            }
            logger.Information($"Processing entity:\n {allMetadata.First().Text}.\n");

            // Batch Embedding
            var allEmbeddings = new List<Embedding<float>>();
            var allTexts = allMetadata.Select(m => m.Text).ToList();
            var batches = SplitList(allTexts, 5);
            logger.Information($"Total texts to embed: {allTexts.Count} in {batches.Count} batches.");
            try
            {
                foreach (var batch in batches)
                {
                    logger.Information($"Generating embeddings for batch of size {batch.Count}...");
                    var embed = await embedder.GenerateAsync(batch);
                    logger.Information($"Embeddings generated, waiting 60 seconds.");

                    await Task.Delay(1000 * 61);

                    foreach (var e in embed)
                    {
                        allEmbeddings.Add(e);
                    }
                }
                logger.Information($"Embeddings done");
            }
            catch (Exception ex)
            {
                logger.Error($"Error during embedding generation: {ex.Message}");
                throw;
            }

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
