using NUnitAssert = NUnit.Framework.Assert;
using DataInjection.Core.Interfaces;
using DataInjection.Core.Providers;
using NSubstitute;
using NUnit.Framework;

namespace nam.ServerTests.DataInjection.Core
{
    [TestFixture]
    public class AbstractProviderTests
    {
        private IFetcher _fetcher = null!;
        private IDtoMapper<TestDto, TestEntity> _mapper = null!;
        private TestProvider _provider = null!;

        [SetUp]
        public void Setup()
        {
            _fetcher = Substitute.For<IFetcher>();
            _mapper = Substitute.For<IDtoMapper<TestDto, TestEntity>>();
        }

        [Test]
        public async Task GetEntity_FetchesAndMapsDto_WhenCalled()
        {
            // Arrange
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?> { { "key", "value" } };
            _provider = new TestProvider(_fetcher, _mapper, endpoint, query);

            var dto = new TestDto { Id = 1, Name = "Test" };
            var entity = new TestEntity { Id = 1, DisplayName = "Test Entity" };

            _fetcher.Fetch<TestDto>(Arg.Any<string>(), endpoint, query, Arg.Any<CancellationToken>())
                .Returns(dto);
            _mapper.MapToEntity(dto).Returns(entity);

            // Act
            var result = await _provider.GetEntity();

            // Assert
            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result.Id, Is.EqualTo(1));
            NUnitAssert.That(result.DisplayName, Is.EqualTo("Test Entity"));
        }

        [Test]
        public async Task GetEntity_CallsFetcherWithCorrectParameters()
        {
            // Arrange
            var endpoint = "/api/users";
            var query = new Dictionary<string, string?> 
            { 
                { "page", "1" },
                { "limit", "10" }
            };
            _provider = new TestProvider(_fetcher, _mapper, endpoint, query);

            var dto = new TestDto { Id = 1, Name = "Test" };
            var entity = new TestEntity { Id = 1, DisplayName = "Test" };

            _fetcher.Fetch<TestDto>(Arg.Any<string>(), endpoint, query, Arg.Any<CancellationToken>())
                .Returns(dto);
            _mapper.MapToEntity(dto).Returns(entity);

            // Act
            await _provider.GetEntity();

            // Assert
            await _fetcher.Received(1).Fetch<TestDto>(
                "https://test.example.com",
                endpoint,
                query,
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task GetEntity_CallsMapperWithFetchedDto()
        {
            // Arrange
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?>();
            _provider = new TestProvider(_fetcher, _mapper, endpoint, query);

            var dto = new TestDto { Id = 42, Name = "Specific" };
            var entity = new TestEntity { Id = 42, DisplayName = "Mapped" };

            _fetcher.Fetch<TestDto>(Arg.Any<string>(), endpoint, query, Arg.Any<CancellationToken>())
                .Returns(dto);
            _mapper.MapToEntity(dto).Returns(entity);

            // Act
            await _provider.GetEntity();

            // Assert
            _mapper.Received(1).MapToEntity(dto);
        }

        [Test]
        public async Task GetEntity_PassesCancellationToken_WhenProvided()
        {
            // Arrange
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?>();
            _provider = new TestProvider(_fetcher, _mapper, endpoint, query);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var dto = new TestDto { Id = 1, Name = "Test" };
            var entity = new TestEntity { Id = 1, DisplayName = "Test" };

            _fetcher.Fetch<TestDto>(Arg.Any<string>(), endpoint, query, cancellationToken)
                .Returns(dto);
            _mapper.MapToEntity(dto).Returns(entity);

            // Act
            await _provider.GetEntity(cancellationToken);

            // Assert
            await _fetcher.Received(1).Fetch<TestDto>(
                Arg.Any<string>(),
                endpoint,
                query,
                cancellationToken);
        }

        [Test]
        public async Task GetEntity_HandlesEmptyQuery()
        {
            // Arrange
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?>();
            _provider = new TestProvider(_fetcher, _mapper, endpoint, query);

            var dto = new TestDto { Id = 1, Name = "Test" };
            var entity = new TestEntity { Id = 1, DisplayName = "Test" };

            _fetcher.Fetch<TestDto>(Arg.Any<string>(), endpoint, query, Arg.Any<CancellationToken>())
                .Returns(dto);
            _mapper.MapToEntity(dto).Returns(entity);

            // Act
            var result = await _provider.GetEntity();

            // Assert
            NUnitAssert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetEntity_HandlesQueryWithNullValues()
        {
            // Arrange
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?> 
            { 
                { "param1", "value1" },
                { "param2", null }
            };
            _provider = new TestProvider(_fetcher, _mapper, endpoint, query);

            var dto = new TestDto { Id = 1, Name = "Test" };
            var entity = new TestEntity { Id = 1, DisplayName = "Test" };

            _fetcher.Fetch<TestDto>(Arg.Any<string>(), endpoint, query, Arg.Any<CancellationToken>())
                .Returns(dto);
            _mapper.MapToEntity(dto).Returns(entity);

            // Act
            var result = await _provider.GetEntity();

            // Assert
            NUnitAssert.That(result, Is.Not.Null);
        }

        [Test]
        public void Query_ExposesProvidedQueryParameters()
        {
            // Arrange
            var endpoint = "/api/test";
            var query = new Dictionary<string, string?> 
            { 
                { "key1", "value1" },
                { "key2", "value2" }
            };
            _provider = new TestProvider(_fetcher, _mapper, endpoint, query);

            // Act
            var exposedQuery = _provider.Query;

            // Assert
            NUnitAssert.That(exposedQuery, Is.Not.Null);
            NUnitAssert.That(exposedQuery.Count, Is.EqualTo(2));
            NUnitAssert.That(exposedQuery["key1"], Is.EqualTo("value1"));
            NUnitAssert.That(exposedQuery["key2"], Is.EqualTo("value2"));
        }

        // Test implementations
        public class TestProvider : AbstractProvider<TestDto, TestEntity>
        {
            public TestProvider(
                IFetcher fetcher,
                IDtoMapper<TestDto, TestEntity> mapper,
                string endpoint,
                Dictionary<string, string?> query)
                : base(fetcher, mapper, endpoint, query)
            {
            }

            public override string GetBaseUrl()
            {
                return "https://test.example.com";
            }
        }

        public class TestDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class TestEntity
        {
            public int Id { get; set; }
            public string DisplayName { get; set; } = string.Empty;
        }
    }
}
