using NUnitAssert = NUnit.Framework.Assert;
using DataInjection.Core.Interfaces;
using Datainjection.Qdrant.Sync;
using DataInjection.Qdrant.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.VectorData;
using NSubstitute;
using NUnit.Framework;
using Serilog;

namespace nam.ServerTests.DataInjection.Qdrant
{
    [TestFixture]
    public class QdrantEntitySyncTests
    {
        private ILogger _logger = null!;
        private IConfiguration _configuration = null!;
        private VectorStoreCollection<Guid, POIEntity> _store = null!;
        private QdrantEntitySync _syncService = null!;

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger>();
            _store = Substitute.For<VectorStoreCollection<Guid, POIEntity>>();

            // Mock configuration with municipalities
            var configData = new Dictionary<string, string?>
            {
                { "Municipalities:0", "Milano" },
                { "Municipalities:1", "Roma" },
                { "Municipalities:2", "Napoli" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configData)
                .Build();

            _syncService = new QdrantEntitySync(_logger, _configuration, _store);
        }

        [Test]
        public async Task ExecuteSyncAsync_EnsuresCollectionExists()
        {
            // Arrange
            var entityCollector = Substitute.For<IEntityCollector<POIEntity>>();
            entityCollector.GetEntities(Arg.Any<string>()).Returns(new List<POIEntity>());

            // Act
            await _syncService.ExecuteSyncAsync(entityCollector);

            // Assert
            await _store.Received(1).EnsureCollectionExistsAsync(Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task ExecuteSyncAsync_FetchesEntitiesFromAllMunicipalities()
        {
            // Arrange
            var entityCollector = Substitute.For<IEntityCollector<POIEntity>>();
            entityCollector.GetEntities(Arg.Any<string>()).Returns(new List<POIEntity>());

            // Act
            await _syncService.ExecuteSyncAsync(entityCollector);

            // Assert
            await entityCollector.Received(1).GetEntities("Milano");
            await entityCollector.Received(1).GetEntities("Roma");
            await entityCollector.Received(1).GetEntities("Napoli");
        }

        [Test]
        public async Task ExecuteSyncAsync_UpsertsEntities_WhenEntitiesAreFetched()
        {
            // Arrange
            var entityCollector = Substitute.For<IEntityCollector<POIEntity>>();
            
            var entities = new List<POIEntity>
            {
                new POIEntity { Id = Guid.NewGuid(), EntityId = "1", city = "Milano", apiEndpoint = "/test" },
                new POIEntity { Id = Guid.NewGuid(), EntityId = "2", city = "Milano", apiEndpoint = "/test" }
            };
            
            entityCollector.GetEntities("Milano").Returns(entities);
            entityCollector.GetEntities(Arg.Is<string>(s => s != "Milano")).Returns(new List<POIEntity>());

            // Act
            await _syncService.ExecuteSyncAsync(entityCollector);

            // Assert
            await _store.Received(1).UpsertAsync(
                Arg.Is<IEnumerable<POIEntity>>(e => e.Count() >= 2),
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task ExecuteSyncAsync_LogsWarning_WhenNoEntitiesFetched()
        {
            // Arrange
            var entityCollector = Substitute.For<IEntityCollector<POIEntity>>();
            entityCollector.GetEntities(Arg.Any<string>()).Returns(new List<POIEntity>());

            // Act
            await _syncService.ExecuteSyncAsync(entityCollector);

            // Assert
            _logger.Received().Warning(Arg.Is<string>(s => s.Contains("No entities were fetched")));
        }

        [Test]
        public async Task ExecuteSyncAsync_DoesNotUpsert_WhenNoEntitiesFetched()
        {
            // Arrange
            var entityCollector = Substitute.For<IEntityCollector<POIEntity>>();
            entityCollector.GetEntities(Arg.Any<string>()).Returns(new List<POIEntity>());

            // Act
            await _syncService.ExecuteSyncAsync(entityCollector);

            // Assert
            await _store.DidNotReceive().UpsertAsync(
                Arg.Any<IEnumerable<POIEntity>>(),
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task ExecuteSyncAsync_HandlesExceptionDuringFetch()
        {
            // Arrange
            var entityCollector = Substitute.For<IEntityCollector<POIEntity>>();
            
            // First municipality throws exception
            entityCollector.GetEntities("Milano")
                .Returns(Task.FromException<List<POIEntity>>(new Exception("Network error")));
            
            // Others succeed with empty lists
            entityCollector.GetEntities("Roma").Returns(new List<POIEntity>());
            entityCollector.GetEntities("Napoli").Returns(new List<POIEntity>());

            // Act - should not throw
            await _syncService.ExecuteSyncAsync(entityCollector);

            // Assert - verify that Error was called (the exact signature may vary)
            _logger.Received().Error(
                Arg.Is<string>(s => s.Contains("Error fetching data")),
                Arg.Any<string>(),
                Arg.Any<string>());
        }

        [Test]
        public async Task ExecuteSyncAsync_ContinuesFetching_AfterOneFailure()
        {
            // Arrange
            var entityCollector = Substitute.For<IEntityCollector<POIEntity>>();
            
            entityCollector.GetEntities("Milano")
                .Returns(Task.FromException<List<POIEntity>>(new Exception("Network error")));
            
            var romaEntities = new List<POIEntity>
            {
                new POIEntity { Id = Guid.NewGuid(), EntityId = "1", city = "Roma", apiEndpoint = "/test" }
            };
            entityCollector.GetEntities("Roma").Returns(romaEntities);
            entityCollector.GetEntities("Napoli").Returns(new List<POIEntity>());

            // Act
            await _syncService.ExecuteSyncAsync(entityCollector);

            // Assert - should still upsert the successful entities
            await _store.Received(1).UpsertAsync(
                Arg.Is<IEnumerable<POIEntity>>(e => e.Any()),
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task ExecuteSyncAsync_CombinesEntitiesFromAllMunicipalities()
        {
            // Arrange
            var entityCollector = Substitute.For<IEntityCollector<POIEntity>>();
            
            var milanoEntities = new List<POIEntity>
            {
                new POIEntity { Id = Guid.NewGuid(), EntityId = "1", city = "Milano", apiEndpoint = "/test" }
            };
            
            var romaEntities = new List<POIEntity>
            {
                new POIEntity { Id = Guid.NewGuid(), EntityId = "2", city = "Roma", apiEndpoint = "/test" },
                new POIEntity { Id = Guid.NewGuid(), EntityId = "3", city = "Roma", apiEndpoint = "/test" }
            };
            
            entityCollector.GetEntities("Milano").Returns(milanoEntities);
            entityCollector.GetEntities("Roma").Returns(romaEntities);
            entityCollector.GetEntities("Napoli").Returns(new List<POIEntity>());

            // Act
            await _syncService.ExecuteSyncAsync(entityCollector);

            // Assert - should upsert all 3 entities
            await _store.Received(1).UpsertAsync(
                Arg.Is<IEnumerable<POIEntity>>(e => e.Count() == 3),
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task ExecuteSyncAsync_HandlesNullEntitiesFromCollector()
        {
            // Arrange
            var entityCollector = Substitute.For<IEntityCollector<POIEntity>>();
            
            // Return null for one municipality
            entityCollector.GetEntities("Milano").Returns((List<POIEntity>)null!);
            entityCollector.GetEntities("Roma").Returns(new List<POIEntity>());
            entityCollector.GetEntities("Napoli").Returns(new List<POIEntity>());

            // Act - should handle gracefully
            await _syncService.ExecuteSyncAsync(entityCollector);

            // Assert - should not crash
            _logger.Received().Warning(Arg.Is<string>(s => s.Contains("No entities were fetched")));
        }

        [Test]
        public async Task ExecuteSyncAsync_HandlesEmptyMunicipalitiesList()
        {
            // Arrange
            var emptyConfig = new ConfigurationBuilder().Build();
            var syncService = new QdrantEntitySync(_logger, emptyConfig, _store);
            var entityCollector = Substitute.For<IEntityCollector<POIEntity>>();

            // Act
            await syncService.ExecuteSyncAsync(entityCollector);

            // Assert - should log warning but not crash
            _logger.Received().Warning(Arg.Any<string>());
        }
    }
}
