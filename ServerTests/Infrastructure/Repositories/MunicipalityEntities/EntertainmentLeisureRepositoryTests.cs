using Domain.Entities.MunicipalityEntities;
using Infrastructure;
using Infrastructure.Repositories.Implemented.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.Infrastructure.Repositories.MunicipalityEntities
{
    [TestFixture]
    public class EntertainmentLeisureRepositoryTests
    {
        private static DbContextOptions<ApplicationDbContext> CreateOptions()
        {
            var databaseName = $"EntertainmentLeisureRepositoryTests_{NUnit.Framework.TestContext.CurrentContext.Test.Name}";
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        private static EntertainmentLeisureDetail CreateDetail(Guid entityId, string municipalityName = "Milano")
        {
            return new EntertainmentLeisureDetail
            {
                Identifier = entityId,
                OfficialName = "Official",
                MunicipalityData = new MunicipalityForLocalStorageSetting { Name = municipalityName }
            };
        }

        private static EntertainmentLeisureCard CreateCard(Guid entityId, string municipalityName = "Milano")
        {
            return new EntertainmentLeisureCard
            {
                EntityId = entityId,
                EntityName = "Park",
                ImagePath = "image.png",
                BadgeText = "Badge",
                Address = "Address",
                Detail = CreateDetail(entityId, municipalityName)
            };
        }

        [Test]
        public async Task GetByEntityIdAsync_ReturnsMatchingCard()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("12121212-1212-1212-1212-121212121212");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.EntertainmentLeisureCards.Add(CreateCard(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new EntertainmentLeisureRepository(context);

            var result = await repository.GetByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.EntityId, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetByEntityIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("13131313-1313-1313-1313-131313131313");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new EntertainmentLeisureRepository(context);

            var result = await repository.GetByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsEmpty_WhenBlank()
        {
            var options = CreateOptions();

            await using var context = new ApplicationDbContext(options);
            var repository = new EntertainmentLeisureRepository(context);

            var result = await repository.GetByMunicipalityNameAsync(" ");

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsMatchingCards()
        {
            var options = CreateOptions();
            var milanoId = Guid.Parse("14141414-1414-1414-1414-141414141414");
            var romaId = Guid.Parse("15151515-1515-1515-1515-151515151515");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.EntertainmentLeisureCards.Add(CreateCard(milanoId, "Milano"));
                setupContext.EntertainmentLeisureCards.Add(CreateCard(romaId, "Roma"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new EntertainmentLeisureRepository(context);

            var result = (await repository.GetByMunicipalityNameAsync("Milano", cancellationToken)).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].EntityId, Is.EqualTo(milanoId));
        }

        [Test]
        public async Task GetDetailByEntityIdAsync_ReturnsDetail()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("16161616-1616-1616-1616-161616161616");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.EntertainmentLeisureDetails.Add(CreateDetail(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new EntertainmentLeisureRepository(context);

            var result = await repository.GetDetailByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetDetailByEntityIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("17171717-1717-1717-1717-171717171717");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new EntertainmentLeisureRepository(context);

            var result = await repository.GetDetailByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetFullEntityListById_AssignsDetails()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("18181818-1818-1818-1818-181818181818");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.EntertainmentLeisureCards.Add(CreateCard(entityId, "Milano"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new EntertainmentLeisureRepository(context);

            var result = (await repository.GetFullEntityListById("Milano", cancellationToken)).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Detail, Is.Not.Null);
            NUnitAssert.That(result[0].Detail!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetFullEntityByIdAsync_SetsDetail()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("19191919-1919-1919-1919-191919191919");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.EntertainmentLeisureCards.Add(new EntertainmentLeisureCard
                {
                    EntityId = entityId,
                    EntityName = "Park",
                    ImagePath = "image.png",
                    BadgeText = "Badge",
                    Address = "Address"
                });
                setupContext.EntertainmentLeisureDetails.Add(CreateDetail(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new EntertainmentLeisureRepository(context);

            var result = await repository.GetFullEntityByIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.Detail, Is.Not.Null);
            NUnitAssert.That(result.Detail!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetFullEntityByIdAsync_Throws_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("20202020-2020-2020-2020-202020202021");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new EntertainmentLeisureRepository(context);

            var exception = NUnitAssert.ThrowsAsync<NullReferenceException>(async () =>
                await repository.GetFullEntityByIdAsync(entityId, cancellationToken));

            NUnitAssert.That(exception, Is.Not.Null);
        }
    }
}
