using Domain.Entities.MunicipalityEntities;
using Infrastructure;
using Infrastructure.Repositories.Implemented.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.Infrastructure.Repositories.MunicipalityEntities
{
    [TestFixture]
    public class RouteRepositoryTests
    {
        private static DbContextOptions<ApplicationDbContext> CreateOptions()
        {
            var databaseName = $"RouteRepositoryTests_{NUnit.Framework.TestContext.CurrentContext.Test.Name}";
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        private static RouteDetail CreateDetail(Guid entityId, string municipalityName = "Milano")
        {
            return new RouteDetail
            {
                Identifier = entityId,
                Name = "Route",
                MunicipalityData = new MunicipalityForLocalStorageSetting { Name = municipalityName }
            };
        }

        private static RouteCard CreateCard(Guid entityId, string municipalityName = "Milano")
        {
            return new RouteCard
            {
                EntityId = entityId,
                EntityName = "Route",
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
            var entityId = Guid.Parse("39393939-3939-3939-3939-393939393939");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.RouteCards.Add(CreateCard(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new RouteRepository(context);

            var result = await repository.GetByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.EntityId, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetByEntityIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("40404040-4040-4040-4040-404040404041");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new RouteRepository(context);

            var result = await repository.GetByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsEmpty_WhenBlank()
        {
            var options = CreateOptions();

            await using var context = new ApplicationDbContext(options);
            var repository = new RouteRepository(context);

            var result = await repository.GetByMunicipalityNameAsync(" ");

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsMatchingCards()
        {
            var options = CreateOptions();
            var milanoId = Guid.Parse("41414141-4141-4141-4141-414141414141");
            var romaId = Guid.Parse("42424242-4242-4242-4242-424242424242");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.RouteCards.Add(CreateCard(milanoId, "Milano"));
                setupContext.RouteCards.Add(CreateCard(romaId, "Roma"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new RouteRepository(context);

            var result = (await repository.GetByMunicipalityNameAsync("Milano", cancellationToken)).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].EntityId, Is.EqualTo(milanoId));
        }

        [Test]
        public async Task GetDetailByEntityIdAsync_ReturnsDetail()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("43434343-4343-4343-4343-434343434343");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.RouteDetails.Add(CreateDetail(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new RouteRepository(context);

            var result = await repository.GetDetailByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetDetailByEntityIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("44444444-4444-4444-4444-444444444445");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new RouteRepository(context);

            var result = await repository.GetDetailByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetFullEntityListById_AssignsDetails()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("45454545-4545-4545-4545-454545454545");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.RouteCards.Add(CreateCard(entityId, "Milano"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new RouteRepository(context);

            var result = (await repository.GetFullEntityListById("Milano", cancellationToken)).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Detail, Is.Not.Null);
            NUnitAssert.That(result[0].Detail!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetFullEntityByIdAsync_SetsDetail()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("46464646-4646-4646-4646-464646464646");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.RouteCards.Add(new RouteCard
                {
                    EntityId = entityId,
                    EntityName = "Route",
                    ImagePath = "image.png",
                    BadgeText = "Badge",
                    Address = "Address"
                });
                setupContext.RouteDetails.Add(CreateDetail(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new RouteRepository(context);

            var result = await repository.GetFullEntityByIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.Detail, Is.Not.Null);
            NUnitAssert.That(result.Detail!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetFullEntityByIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("47474747-4747-4747-4747-474747474747");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new RouteRepository(context);

            var result = await repository.GetFullEntityByIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }
    }
}
