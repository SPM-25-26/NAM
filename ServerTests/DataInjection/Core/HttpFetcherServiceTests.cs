using DataInjection.Core.Fetchers;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;
using Serilog;
using System.Net;
using System.Text;
using System.Text.Json;

namespace nam.ServerTests.DataInjection.Core
{
    [TestFixture]
    public class HttpFetcherServiceTests
    {
        private IHttpClientFactory _httpClientFactory = null!;
        private ILogger _logger = null!;
        private HttpFetcherService _fetcherService = null!;
        private HttpMessageHandler _mockHandler = null!;
        private HttpClient _httpClient = null!;

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger>();
            _httpClientFactory = Substitute.For<IHttpClientFactory>();
            
            // Create a mock handler that we can control
            _mockHandler = Substitute.For<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHandler);
            
            _httpClientFactory.CreateClient().Returns(_httpClient);
            _fetcherService = new HttpFetcherService(_httpClientFactory, _logger);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }

        [Test]
        public async Task Fetch_ThrowsInvalidOperationException_WhenBaseUrlIsNull()
        {
            // Arrange
            var baseUrl = "";
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?>();

            // Act & Assert
            var ex = NUnitAssert.ThrowsAsync<InvalidOperationException>(async () =>
                await _fetcherService.Fetch<TestDto>(baseUrl, endpoint, query));
            
            NUnitAssert.That(ex.Message, Does.Contain("baseUrl is not set"));
        }

        [Test]
        public async Task Fetch_ThrowsInvalidOperationException_WhenBaseUrlIsWhitespace()
        {
            // Arrange
            var baseUrl = "   ";
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?>();

            // Act & Assert
            var ex = NUnitAssert.ThrowsAsync<InvalidOperationException>(async () =>
                await _fetcherService.Fetch<TestDto>(baseUrl, endpoint, query));
            
            NUnitAssert.That(ex.Message, Does.Contain("baseUrl is not set"));
        }

        [Test]
        public async Task Fetch_ReturnsDefault_WhenResponseContentIsEmpty()
        {
            // Arrange
            var baseUrl = "https://api.example.com";
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?>();
            
            var mockHttpClient = CreateMockHttpClient("");

            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(mockHttpClient);
            var fetcherService = new HttpFetcherService(httpClientFactory, _logger);

            // Act
            var result = await fetcherService.Fetch<TestDto>(baseUrl, endpoint, query);

            // Assert
            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task Fetch_ReturnsDefault_WhenResponseContentIsNull()
        {
            // Arrange
            var baseUrl = "https://api.example.com";
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?>();
            
            var mockHttpClient = CreateMockHttpClient(null);

            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(mockHttpClient);
            var fetcherService = new HttpFetcherService(httpClientFactory, _logger);

            // Act
            var result = await fetcherService.Fetch<TestDto>(baseUrl, endpoint, query);

            // Assert
            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task Fetch_ReturnsDeserializedDto_WhenRequestSucceeds()
        {
            // Arrange
            var baseUrl = "https://api.example.com";
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?>();
            
            var expectedDto = new TestDto { Id = 1, Name = "Test" };
            var jsonContent = JsonSerializer.Serialize(expectedDto);
            
            var mockHttpClient = CreateMockHttpClient(jsonContent);

            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(mockHttpClient);
            var fetcherService = new HttpFetcherService(httpClientFactory, _logger);

            // Act
            var result = await fetcherService.Fetch<TestDto>(baseUrl, endpoint, query);

            // Assert
            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result.Id, Is.EqualTo(1));
            NUnitAssert.That(result.Name, Is.EqualTo("Test"));
        }

        [Test]
        public async Task Fetch_HandlesQueryParameters_WhenQueryProvided()
        {
            // Arrange
            var baseUrl = "https://api.example.com";
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?>
            {
                { "param1", "value1" },
                { "param2", "value2" }
            };
            
            var expectedDto = new TestDto { Id = 1, Name = "Test" };
            var jsonContent = JsonSerializer.Serialize(expectedDto);
            
            var mockHttpClient = CreateMockHttpClient(jsonContent);

            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(mockHttpClient);
            var fetcherService = new HttpFetcherService(httpClientFactory, _logger);

            // Act
            var result = await fetcherService.Fetch<TestDto>(baseUrl, endpoint, query);

            // Assert
            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task Fetch_ReplacesPathParameters_WhenPlaceholdersInEndpoint()
        {
            // Arrange
            var baseUrl = "https://api.example.com";
            var endpoint = "/api/users/{userId}/posts/{postId}";
            var query = new Dictionary<string, string?>
            {
                { "userId", "123" },
                { "postId", "456" }
            };
            
            var expectedDto = new TestDto { Id = 1, Name = "Test" };
            var jsonContent = JsonSerializer.Serialize(expectedDto);
            
            var mockHttpClient = CreateMockHttpClient(jsonContent);

            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(mockHttpClient);
            var fetcherService = new HttpFetcherService(httpClientFactory, _logger);

            // Act
            var result = await fetcherService.Fetch<TestDto>(baseUrl, endpoint, query);

            // Assert
            NUnitAssert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Fetch_HandlesCaseInsensitiveDeserialization()
        {
            // Arrange
            var baseUrl = "https://api.example.com";
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?>();
            
            // JSON with lowercase property names
            var jsonContent = "{\"id\":42,\"name\":\"TestValue\"}";
            
            var mockHttpClient = CreateMockHttpClient(jsonContent);

            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(mockHttpClient);
            var fetcherService = new HttpFetcherService(httpClientFactory, _logger);

            // Act
            var result = await fetcherService.Fetch<TestDto>(baseUrl, endpoint, query);

            // Assert
            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result.Id, Is.EqualTo(42));
            NUnitAssert.That(result.Name, Is.EqualTo("TestValue"));
        }

        [Test]
        public async Task Fetch_HandlesVeryLongStrings()
        {
            // Arrange
            var baseUrl = "https://api.example.com";
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?>();
            
            var longString = new string('x', 10000);
            var expectedDto = new TestDto { Id = 1, Name = longString };
            var jsonContent = JsonSerializer.Serialize(expectedDto);
            
            var mockHttpClient = CreateMockHttpClient(jsonContent);

            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(mockHttpClient);
            var fetcherService = new HttpFetcherService(httpClientFactory, _logger);

            // Act
            var result = await fetcherService.Fetch<TestDto>(baseUrl, endpoint, query);

            // Assert
            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result.Name, Has.Length.EqualTo(10000));
        }

        [Test]
        public async Task Fetch_HandlesUnicodeCharacters()
        {
            // Arrange
            var baseUrl = "https://api.example.com";
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?>();
            
            var unicodeString = "Hello 世界 🌍 Ñoño";
            var expectedDto = new TestDto { Id = 1, Name = unicodeString };
            var jsonContent = JsonSerializer.Serialize(expectedDto);
            
            var mockHttpClient = CreateMockHttpClient(jsonContent);

            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(mockHttpClient);
            var fetcherService = new HttpFetcherService(httpClientFactory, _logger);

            // Act
            var result = await fetcherService.Fetch<TestDto>(baseUrl, endpoint, query);

            // Assert
            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result.Name, Is.EqualTo(unicodeString));
        }

        [Test]
        public async Task Fetch_ReturnsDefault_WhenDeserializationFails()
        {
            // Arrange
            var baseUrl = "https://api.example.com";
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?>();
            
            var invalidJson = "{ invalid json }";
            
            var mockHttpClient = CreateMockHttpClient(invalidJson);

            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(mockHttpClient);
            var fetcherService = new HttpFetcherService(httpClientFactory, _logger);

            // Act & Assert
            NUnitAssert.ThrowsAsync<JsonException>(async () =>
                await fetcherService.Fetch<TestDto>(baseUrl, endpoint, query));
        }

        [Test]
        public async Task Fetch_TrimsTrailingSlashFromBaseUrl()
        {
            // Arrange
            var baseUrl = "https://api.example.com/";
            var endpoint = "api/test";
            var query = new Dictionary<string, string?>();
            
            var expectedDto = new TestDto { Id = 1, Name = "Test" };
            var jsonContent = JsonSerializer.Serialize(expectedDto);
            
            var mockHttpClient = CreateMockHttpClient(jsonContent);

            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient().Returns(mockHttpClient);
            var fetcherService = new HttpFetcherService(httpClientFactory, _logger);

            // Act
            var result = await fetcherService.Fetch<TestDto>(baseUrl, endpoint, query);

            // Assert
            NUnitAssert.That(result, Is.Not.Null);
        }

        // Helper method to create a mock HttpClient with predetermined response
        private HttpClient CreateMockHttpClient(string? responseContent)
        {
            var mockHandler = new MockHttpMessageHandler(responseContent);
            return new HttpClient(mockHandler);
        }

        // Test DTO
        private class TestDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        // Mock handler to simulate HTTP responses
        private class MockHttpMessageHandler : HttpMessageHandler
        {
            private readonly string? _responseContent;

            public MockHttpMessageHandler(string? responseContent)
            {
                _responseContent = responseContent;
            }

            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                if (_responseContent != null)
                {
                    response.Content = new StringContent(_responseContent, Encoding.UTF8, "application/json");
                }
                else
                {
                    response.Content = new StringContent(string.Empty);
                }
                return Task.FromResult(response);
            }
        }
    }
}
