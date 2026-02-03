using Domain.Entities.MunicipalityEntities;
using Infrastructure;
using Infrastructure.Repositories.Implemented.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.Infrastructure.Repositories.MunicipalityEntities
{
    [TestFixture]
    public class ArtCultureRepositoryTests
    {
        private static DbContextOptions<ApplicationDbContext> CreateOptions()
        {
            var databaseName = $"ArtCultureRepositoryTests_{NUnit.Framework.TestContext.CurrentContext.Test.Name}";
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        private static ArtCultureNatureDetail CreateDetail(Guid entityId, string municipalityName = "Milano")
        {
            return new ArtCultureNatureDetail
            {
                Identifier = entityId,
                OfficialName = "Official",
                MunicipalityData = new MunicipalityForLocalStorageSetting { Name = municipalityName }
            };
        }

        private static ArtCultureNatureCard CreateCard(Guid entityId, string municipalityName = "Milano")
        {
            return new ArtCultureNatureCard
            {
                EntityId = entityId,
                EntityName = "Museum",
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
            var entityId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.ArtCultureNatureCards.Add(CreateCard(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new ArtCultureRepository(context);

            var result = await repository.GetByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.EntityId, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetByEntityIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new ArtCultureRepository(context);

            var result = await repository.GetByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsEmpty_WhenBlank()
        {
            var options = CreateOptions();

            await using var context = new ApplicationDbContext(options);
            var repository = new ArtCultureRepository(context);

            var result = await repository.GetByMunicipalityNameAsync(" ");

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsMatchingCards()
        {
            var options = CreateOptions();
            var milanoId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var romaId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.ArtCultureNatureCards.Add(CreateCard(milanoId, "Milano"));
                setupContext.ArtCultureNatureCards.Add(CreateCard(romaId, "Roma"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new ArtCultureRepository(context);

            var result = (await repository.GetByMunicipalityNameAsync("Milano", cancellationToken)).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].EntityId, Is.EqualTo(milanoId));
        }

        [Test]
        public async Task GetDetailByEntityIdAsync_ReturnsDetail()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.ArtCultureNatureDetails.Add(CreateDetail(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new ArtCultureRepository(context);

            var result = await repository.GetDetailByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetDetailByEntityIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("66666666-6666-6666-6666-666666666666");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new ArtCultureRepository(context);

            var result = await repository.GetDetailByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetFullEntityListById_AssignsDetails()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("77777777-7777-7777-7777-777777777777");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.ArtCultureNatureCards.Add(CreateCard(entityId, "Milano"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new ArtCultureRepository(context);

            var result = (await repository.GetFullEntityListById("Milano", cancellationToken)).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Detail, Is.Not.Null);
            NUnitAssert.That(result[0].Detail!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetFullEntityByIdAsync_SetsDetail()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("88888888-8888-8888-8888-888888888888");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.ArtCultureNatureCards.Add(new ArtCultureNatureCard
                {
                    EntityId = entityId,
                    EntityName = "Museum",
                    ImagePath = "image.png",
                    BadgeText = "Badge",
                    Address = "Address"
                });
                setupContext.ArtCultureNatureDetails.Add(CreateDetail(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new ArtCultureRepository(context);

            var result = await repository.GetFullEntityByIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.Detail, Is.Not.Null);
            NUnitAssert.That(result.Detail!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetFullEntityByIdAsync_Throws_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("99999999-9999-9999-9999-999999999999");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new ArtCultureRepository(context);

            var exception = NUnitAssert.ThrowsAsync<NullReferenceException>(async () =>
                await repository.GetFullEntityByIdAsync(entityId, cancellationToken));

            NUnitAssert.That(exception, Is.Not.Null);
        }
    }
}
