using Datainjection.Qdrant.Sync;
using DataInjection.Core.Interfaces;
using DataInjection.Qdrant.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.VectorData;
using Moq;
using NUnit.Framework;
using Serilog;

namespace nam.ServerTests.DataInjection.Qdrant
{
    [TestFixture]
    public class QdrantEntitySyncTests
    {
        private Mock<ILogger> _mockLogger;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<VectorStoreCollection<Guid, POIEntity>> _mockStore;
        private Mock<IEntityCollector<POIEntity>> _mockCollector1;
        private Mock<IEntityCollector<POIEntity>> _mockCollector2;
        private QdrantEntitySync _sut;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockStore = new Mock<VectorStoreCollection<Guid, POIEntity>>();
            _mockCollector1 = new Mock<IEntityCollector<POIEntity>>();
            _mockCollector2 = new Mock<IEntityCollector<POIEntity>>();

            _mockCollector1.Setup(c => c.ToString()).Returns("Collector1");
            _mockCollector2.Setup(c => c.ToString()).Returns("Collector2");
        }

        [Test]
        public async Task ExecuteSyncAsync_WithEmptyMunicipalities_LogsWarningAndReturnsEarly()
        {
            // Arrange
            var emptyMunicipalities = Array.Empty<string>();
            SetupMunicipalities(emptyMunicipalities);

            _sut = new QdrantEntitySync(
                _mockLogger.Object,
                _mockConfiguration.Object,
                _mockStore.Object,
                [_mockCollector1.Object]
            );

            // Act
            await _sut.ExecuteSyncAsync();

            // Assert
            _mockStore.Verify(s => s.EnsureCollectionExistsAsync(default), Times.Once);
            _mockLogger.Verify(
                l => l.Warning(It.Is<string>(s => s.Contains("Empty list of municipalities"))),
                Times.Once
            );
            _mockCollector1.Verify(c => c.GetEntities(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task ExecuteSyncAsync_WithNullMunicipalities_LogsWarningAndReturnsEarly()
        {
            // Arrange
            SetupMunicipalities(null);

            _sut = new QdrantEntitySync(
                _mockLogger.Object,
                _mockConfiguration.Object,
                _mockStore.Object,
                [_mockCollector1.Object]
            );

            // Act
            await _sut.ExecuteSyncAsync();

            // Assert
            _mockStore.Verify(s => s.EnsureCollectionExistsAsync(default), Times.Once);
            _mockLogger.Verify(
                l => l.Warning(It.Is<string>(s => s.Contains("Empty list of municipalities"))),
                Times.Once
            );
        }

        [Test]
        public async Task ExecuteSyncAsync_WithValidMunicipalities_ProcessesAllCollectorsAndMunicipalities()
        {
            // Arrange
            var municipalities = new[] { "Trento", "Bolzano" };
            SetupMunicipalities(municipalities);

            var entities1 = new List<POIEntity>
            {
                new() { Id = Guid.NewGuid(), city = "Trento", EntityId = "1" },
                new() { Id = Guid.NewGuid(), city = "Trento", EntityId = "2" }
            };

            var entities2 = new List<POIEntity>
            {
                new() { Id = Guid.NewGuid(), city = "Bolzano", EntityId = "3" }
            };

            _mockCollector1.Setup(c => c.GetEntities("Trento")).ReturnsAsync(entities1);
            _mockCollector1.Setup(c => c.GetEntities("Bolzano")).ReturnsAsync(entities2);
            _mockCollector2.Setup(c => c.GetEntities("Trento")).ReturnsAsync(entities1);
            _mockCollector2.Setup(c => c.GetEntities("Bolzano")).ReturnsAsync(entities2);

            _sut = new QdrantEntitySync(
                _mockLogger.Object,
                _mockConfiguration.Object,
                _mockStore.Object,
                [_mockCollector1.Object, _mockCollector2.Object]
            );

            // Act
            await _sut.ExecuteSyncAsync();

            // Assert
            _mockStore.Verify(s => s.EnsureCollectionExistsAsync(default), Times.Once);
            _mockCollector1.Verify(c => c.GetEntities("Trento"), Times.Once);
            _mockCollector1.Verify(c => c.GetEntities("Bolzano"), Times.Once);
            _mockCollector2.Verify(c => c.GetEntities("Trento"), Times.Once);
            _mockCollector2.Verify(c => c.GetEntities("Bolzano"), Times.Once);

            _mockStore.Verify(s => s.UpsertAsync(It.IsAny<IEnumerable<POIEntity>>(), default), Times.Exactly(4));

            _mockLogger.Verify(
                l => l.Information(It.Is<string>(s => s.Contains("Successfully synced qdrant data")), It.IsAny<object[]>()),
                Times.Exactly(4)
            );

            _mockLogger.Verify(
                l => l.Information(It.Is<string>(s => s.Contains("Successfully injected data of municipality")), It.IsAny<object[]>()),
                Times.Exactly(2)
            );
        }

        [Test]
        public async Task ExecuteSyncAsync_WhenCollectorThrowsException_LogsErrorAndContinues()
        {
            // Arrange
            var municipalities = new[] { "Trento", "Bolzano" };
            SetupMunicipalities(municipalities);

            var validEntities = new List<POIEntity>
            {
                new() { Id = Guid.NewGuid(), city = "Bolzano", EntityId = "1" }
            };

            _mockCollector1.Setup(c => c.GetEntities("Trento"))
                .ThrowsAsync(new HttpRequestException("Network error"));
            _mockCollector1.Setup(c => c.GetEntities("Bolzano"))
                .ReturnsAsync(validEntities);

            _sut = new QdrantEntitySync(
                _mockLogger.Object,
                _mockConfiguration.Object,
                _mockStore.Object,
                [_mockCollector1.Object]
            );

            // Act
            await _sut.ExecuteSyncAsync();

            // Assert
            _mockLogger.Verify(
                l => l.Error(
                    It.Is<string>(s => s.Contains("Error fetching data for municipality")),
                    "Trento",
                    It.Is<string>(s => s.Contains("Network error"))
                ),
                Times.Once
            );

            _mockStore.Verify(s => s.UpsertAsync(validEntities, default), Times.Once);

            _mockLogger.Verify(
                l => l.Information(It.Is<string>(s => s.Contains("Successfully synced qdrant data")), It.IsAny<object[]>()),
                Times.Exactly(2) // Both for Trento (after error) and Bolzano
            );
        }

        [Test]
        public async Task ExecuteSyncAsync_WithMultipleCollectors_ProcessesInOrder()
        {
            // Arrange
            var municipalities = new[] { "Trento" };
            SetupMunicipalities(municipalities);

            var executionOrder = new List<string>();

            _mockCollector1.Setup(c => c.GetEntities("Trento"))
                .ReturnsAsync(new List<POIEntity>())
                .Callback(() => executionOrder.Add("Collector1"));

            _mockCollector2.Setup(c => c.GetEntities("Trento"))
                .ReturnsAsync(new List<POIEntity>())
                .Callback(() => executionOrder.Add("Collector2"));

            _sut = new QdrantEntitySync(
                _mockLogger.Object,
                _mockConfiguration.Object,
                _mockStore.Object,
                [_mockCollector1.Object, _mockCollector2.Object]
            );

            // Act
            await _sut.ExecuteSyncAsync();

            // Assert
            NUnit.Framework.Assert.That(executionOrder, Is.EqualTo(new[] { "Collector1", "Collector2" }));
        }

        [Test]
        public async Task ExecuteSyncAsync_AlwaysEnsuresCollectionExists()
        {
            // Arrange
            SetupMunicipalities(new[] { "Trento" });

            _mockCollector1.Setup(c => c.GetEntities(It.IsAny<string>()))
                .ReturnsAsync(new List<POIEntity>());

            _sut = new QdrantEntitySync(
                _mockLogger.Object,
                _mockConfiguration.Object,
                _mockStore.Object,
                [_mockCollector1.Object]
            );

            // Act
            await _sut.ExecuteSyncAsync();

            // Assert
            _mockStore.Verify(s => s.EnsureCollectionExistsAsync(default), Times.Once);
        }

        private void SetupMunicipalities(string[]? municipalities)
        {
            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(s => s.Get<string[]>()).Returns(municipalities);
            _mockConfiguration.Setup(c => c.GetSection("Municipalities")).Returns(mockSection.Object);
        }
    }
}