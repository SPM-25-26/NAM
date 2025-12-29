using DotnetGeminiSDK.Client.Interfaces;

namespace DataInjection.Qdrant.Embedders
{
    public class GeminiEmbedder : IEmbedder
    {
        private IGeminiClient geminiClient;

        public async Task<List<float[]>> GetBatchEmbeddingAsync(ICollection<string> strings)
        {
            var response = await geminiClient.BatchEmbeddedContentsPrompt(strings.ToList());
            if (response?.Embedding == null)
                throw new InvalidOperationException("Embedding response or values are null.");
            return response.Embedding.Select(e => e.Values
                .OfType<float>()
                .ToArray())
                .ToList();
        }

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            var response = await geminiClient.EmbeddedContentsPrompt(text);
            if (response?.Embedding == null || response.Embedding.Values == null)
                throw new InvalidOperationException("Embedding response or values are null.");
            return response.Embedding.Values
                .OfType<float>()
                .ToArray();
        }
    }
}
