using DataInjection.Qdrant.Data;
using Domain.Entities;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Moq;
using Moq.Protected;
using nam.Server.Services.Implemented.Chatbot;
using Serilog;
using System.Net;
using System.Text;
using System.Text.Json;
namespace nam.ServerTests.mock
{
    public class ChatbotServiceTestBuilder : IDisposable
    {
        private readonly Mock<ILogger> _mockLogger;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IChatCompletionService> _mockChatService;
        private readonly Mock<IEmbeddingGenerator<string, Embedding<float>>> _mockEmbedder;
        private readonly Mock<VectorStoreCollection<Guid, POIEntity>> _mockStore;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;

        private Dictionary<string, HttpResponseMessage> _apiResponses;
        private List<VectorSearchResult<POIEntity>> _vectorSearchResults;
        private string _chatCompletionResponse;

        public ChatbotServiceTestBuilder()
        {
            _mockLogger = new Mock<ILogger>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockChatService = new Mock<IChatCompletionService>();
            _mockEmbedder = new Mock<IEmbeddingGenerator<string, Embedding<float>>>();
            _mockStore = new Mock<VectorStoreCollection<Guid, POIEntity>>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            _apiResponses = new Dictionary<string, HttpResponseMessage>();
            _vectorSearchResults = new List<VectorSearchResult<POIEntity>>();
            _chatCompletionResponse = "Default response";

            SetupHttpClient();
            SetupDefaultMocks();
        }

        private void SetupHttpClient()
        {
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    var uri = request.RequestUri?.ToString() ?? "";
                    foreach (var kvp in _apiResponses)
                    {
                        if (uri.Contains(kvp.Key))
                        {
                            return kvp.Value;
                        }
                    }
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                });

            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:5000")
            };

            _mockHttpClientFactory
                .Setup(f => f.CreateClient("entities-api"))
                .Returns(_httpClient);
        }

        private void SetupDefaultMocks()
        {
            // Setup default embedding response
            var defaultEmbedding = new Embedding<float>(new float[] { 0.1f, 0.2f, 0.3f });
            var embeddingsResult = new GeneratedEmbeddings<Embedding<float>>([defaultEmbedding]);
            
            _mockEmbedder
                .Setup(e => e.GenerateAsync(
                    It.IsAny<IEnumerable<string>>(), 
                    It.IsAny<EmbeddingGenerationOptions>(), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(embeddingsResult);

            // Setup default chat completion
            _mockChatService
                .Setup(c => c.GetChatMessageContentsAsync(
                    It.IsAny<ChatHistory>(),
                    It.IsAny<PromptExecutionSettings>(),
                    It.IsAny<Kernel>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new List<ChatMessageContent> 
                { 
                    new ChatMessageContent(new AuthorRole("assistant"), _chatCompletionResponse) 
                }.AsReadOnly());
        }

        public void SetupQuestionaireResponse(string userEmail, string preferences)
        {
            var questionaire = new Questionaire
            {

            };

            var json = JsonSerializer.Serialize(questionaire);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            _apiResponses["/api/user/questionaire"] = response;
        }

        public void SetupQuestionaireNotFound()
        {
            var questionaire = new Questionaire { };
            var json = JsonSerializer.Serialize(questionaire);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            _apiResponses["/api/user/questionaire"] = response;
        }

        public void SetupQuestionaireApiFailure()
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            _apiResponses["/api/user/questionaire"] = response;
        }

        public void SetupVectorSearchResults(float score, string entityId, string entityType)
        {
            var poiEntity = new POIEntity
            {
                EntityId = entityId,
                apiEndpoint = $"/api/{entityType.ToLower()}"
            };

            var searchResult = new VectorSearchResult<POIEntity>(poiEntity, score);
            _vectorSearchResults.Add(searchResult);

            SetupVectorStore();
        }

        public void SetupMultipleVectorSearchResults(IEnumerable<(float score, string entityId, string entityType, string apiResponse)> results)
        {
            foreach (var result in results)
            {
                var poiEntity = new POIEntity
                {
                    EntityId = result.entityId,
                    apiEndpoint = $"/api/{result.entityType.ToLower()}"
                };

                var searchResult = new VectorSearchResult<POIEntity>(poiEntity, result.score);
                _vectorSearchResults.Add(searchResult);

                SetupEntityApiResponse(result.entityId, result.apiResponse);
            }

            SetupVectorStore();
        }

        public void SetupEmptyVectorSearchResults()
        {
            _vectorSearchResults.Clear();
            SetupVectorStore();
        }

        private void SetupVectorStore()
        {
            var asyncEnumerable = _vectorSearchResults.ToAsyncEnumerable();

            _mockStore
                .Setup(s => s.SearchAsync(
                    It.IsAny<ReadOnlyMemory<float>>(),
                    It.IsAny<int>(),
                    It.IsAny<VectorSearchOptions<POIEntity>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(asyncEnumerable);
        }

        public void SetupEntityApiResponse(string entityId, string content)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            _apiResponses[$"identifier={entityId}"] = response;
        }

        public void SetupEntityApiFailure(string entityId)
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            _apiResponses[$"identifier={entityId}"] = response;
        }

        public void SetupChatCompletionResponse(string response)
        {
            _chatCompletionResponse = response;
        }

        public ChatbotService Build()
        {
            return new ChatbotService(
                _mockLogger.Object,
                _mockHttpClientFactory.Object,
                _mockConfiguration.Object,
                _mockChatService.Object,
                _mockEmbedder.Object,
                _mockStore.Object
            );
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
            foreach (var response in _apiResponses.Values)
            {
                response?.Dispose();
            }
        }
    }

    // Helper extension per convertire IEnumerable in IAsyncEnumerable
    public static class AsyncEnumerableExtensions
    {
        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                yield return item;
            }
            await Task.CompletedTask;
        }
    }
}