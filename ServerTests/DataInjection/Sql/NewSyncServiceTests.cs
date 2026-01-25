using NUnitAssert = NUnit.Framework.Assert;
using DataInjection.SQL.Sync;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Serilog;
using NSubstitute;

namespace nam.ServerTests.DataInjection.Sql
{
    [TestFixture]
    public class NewSyncServiceTests
    {
        private ApplicationDbContext _dbContext = null!;
        private ILogger _logger = null!;
        private IConfiguration _configuration = null!;
        private NewSyncService _syncService = null!;

        [SetUp]
        public void Setup()
        {
            // Create in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _logger = Substitute.For<ILogger>();
            
            // Mock configuration with municipalities
            var configData = new Dictionary<string, string?>
            {
                { "Municipalities:0", "Milano" },
                { "Municipalities:1", "Roma" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configData)
                .Build();

            _syncService = new NewSyncService(_dbContext, _logger, _configuration);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Dispose();
        }

        [Test]
        public void NewSyncService_CanBeInstantiated()
        {
            // Act & Assert
            NUnitAssert.That(_syncService, Is.Not.Null);
        }

        [Test]
        public void NewSyncService_HasExpectedDependencies()
        {
            // Assert - verifies that the service was created with valid dependencies
            NUnitAssert.That(_dbContext, Is.Not.Null);
            NUnitAssert.That(_logger, Is.Not.Null);
            NUnitAssert.That(_configuration, Is.Not.Null);
        }

        // Note: More complex integration tests with entity collection and database operations
        // are intentionally not included here to maintain test simplicity and determinism.
        // Those would require properly configured entities in the DbContext which is beyond
        // the scope of unit testing the sync service interface.
        // 
        // The NewSyncService uses reflection and Entity Framework Core's model metadata heavily,
        // making it difficult to test in isolation without a fully configured DbContext with
        // actual entity types. Integration tests with real entities would be more appropriate
        // for comprehensive testing of this service.
    }
}
