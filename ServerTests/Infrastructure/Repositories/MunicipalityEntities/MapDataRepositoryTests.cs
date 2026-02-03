using Domain.Entities.MunicipalityEntities;
using Infrastructure;
using Infrastructure.Repositories.Implemented.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.Infrastructure.Repositories.MunicipalityEntities
{
    [TestFixture]
    public class MapDataRepositoryTests
    {
        private static DbContextOptions<ApplicationDbContext> CreateOptions()
        {
            var databaseName = $"MapDataRepositoryTests_{NUnit.Framework.TestContext.CurrentContext.Test.Name}";
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsEntity_WhenFound()
        {
            var options = CreateOptions();
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.MapData.Add(new MapData
                {
                    Name = "Milano",
                    CenterLatitude = 45.4,
                    CenterLongitude = 9.1,
                    Marker = new List<MapMarker> { new MapMarker { Name = "Marker", ImagePath = "pin.png" } }
                });
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new MapDataRepository(context);

            var result = await repository.GetByMunicipalityNameAsync("Milano", cancellationToken);

            NUnitAssert.That(result.Name, Is.EqualTo("Milano"));
            NUnitAssert.That(result.Marker, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsEmptyName_WhenBlank()
        {
            var options = CreateOptions();

            await using var context = new ApplicationDbContext(options);
            var repository = new MapDataRepository(context);

            var result = await repository.GetByMunicipalityNameAsync(" ");

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result.Name, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsDefault_WhenMissing()
        {
            var options = CreateOptions();
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new MapDataRepository(context);

            var result = await repository.GetByMunicipalityNameAsync("Roma", cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result.Name, Is.EqualTo("Roma"));
            NUnitAssert.That(result.CenterLatitude, Is.EqualTo(0));
            NUnitAssert.That(result.CenterLongitude, Is.EqualTo(0));
            NUnitAssert.That(result.Marker, Is.Empty);
        }
    }
}
