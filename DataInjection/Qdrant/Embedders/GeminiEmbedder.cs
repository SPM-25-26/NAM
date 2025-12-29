using System.Net.Http.Json;
using System.Text.Json;

namespace DataInjection.Qdrant.Embedders
{
    internal class GeminiEmbedder : IEmbedder
    {
        private static readonly Lazy<GeminiEmbedder> _instance = new(() => new GeminiEmbedder());
        public static GeminiEmbedder Instance => _instance.Value;

        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string GeminiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-embedding-001:embedContent";
        private const int OutputDimensionality = 768;

        private GeminiEmbedder()
        {
            _httpClient = new HttpClient();
            _apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? throw new InvalidOperationException("GEMINI_API_KEY non impostata.");
        }

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            var requestBody = new
            {
                content = new
                {
                    parts = new[]
                    {
                        new { text }
                    }
                },
                output_dimensionality = OutputDimensionality
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, $"{GeminiEndpoint}?key={_apiKey}")
            {
                Content = JsonContent.Create(requestBody)
            };
            request.Headers.Add("Accept", "application/json");

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var contentStream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(contentStream);

            var embedding = doc.RootElement
                .GetProperty("embedding")
                .GetProperty("values");

            var result = new float[embedding.GetArrayLength()];
            int i = 0;
            foreach (var value in embedding.EnumerateArray())
            {
                result[i++] = value.GetSingle();
            }

            return result;
        }
    }
}
