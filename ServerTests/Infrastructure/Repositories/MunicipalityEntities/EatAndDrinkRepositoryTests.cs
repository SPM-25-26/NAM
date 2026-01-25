using Domain.Entities.MunicipalityEntities;
using Infrastructure;
using Infrastructure.Repositories.Implemented.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.Infrastructure.Repositories.MunicipalityEntities
{
    [TestFixture]
    public class EatAndDrinkRepositoryTests
    {
        private static DbContextOptions<ApplicationDbContext> CreateOptions()
        {
            var databaseName = $"EatAndDrinkRepositoryTests_{NUnit.Framework.TestContext.CurrentContext.Test.Name}";
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        private static EatAndDrinkDetail CreateDetail(Guid entityId, string municipalityName = "Milano")
        {
            return new EatAndDrinkDetail
            {
                Identifier = entityId,
                OfficialName = "Official",
                MunicipalityData = new MunicipalityForLocalStorageSetting { Name = municipalityName }
            };
        }

        private static EatAndDrinkCard CreateCard(Guid entityId, string municipalityName = "Milano")
        {
            return new EatAndDrinkCard
            {
                EntityId = entityId,
                EntityName = "Restaurant",
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
            var entityId = Guid.Parse("10101010-1010-1010-1010-101010101010");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.EatAndDrinkCards.Add(CreateCard(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new EatAndDrinkRepository(context);

            var result = await repository.GetByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.EntityId, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetByEntityIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("20202020-2020-2020-2020-202020202020");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new EatAndDrinkRepository(context);

            var result = await repository.GetByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsEmpty_WhenBlank()
        {
            var options = CreateOptions();

            await using var context = new ApplicationDbContext(options);
            var repository = new EatAndDrinkRepository(context);

            var result = await repository.GetByMunicipalityNameAsync(" ");

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsMatchingCards()
        {
            var options = CreateOptions();
            var milanoId = Guid.Parse("30303030-3030-3030-3030-303030303030");
            var romaId = Guid.Parse("40404040-4040-4040-4040-404040404040");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.EatAndDrinkCards.Add(CreateCard(milanoId, "Milano"));
                setupContext.EatAndDrinkCards.Add(CreateCard(romaId, "Roma"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new EatAndDrinkRepository(context);

            var result = (await repository.GetByMunicipalityNameAsync("Milano", cancellationToken)).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].EntityId, Is.EqualTo(milanoId));
        }

        [Test]
        public async Task GetDetailByEntityIdAsync_ReturnsDetail()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("50505050-5050-5050-5050-505050505050");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.EatAndDrinkDetails.Add(CreateDetail(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new EatAndDrinkRepository(context);

            var result = await repository.GetDetailByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetDetailByEntityIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("60606060-6060-6060-6060-606060606060");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new EatAndDrinkRepository(context);

            var result = await repository.GetDetailByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetFullEntityListById_AssignsDetails()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("70707070-7070-7070-7070-707070707070");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.EatAndDrinkCards.Add(CreateCard(entityId, "Milano"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new EatAndDrinkRepository(context);

            var result = (await repository.GetFullEntityListById("Milano", cancellationToken)).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Detail, Is.Not.Null);
            NUnitAssert.That(result[0].Detail!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetFullEntityByIdAsync_SetsDetail()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("80808080-8080-8080-8080-808080808080");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.EatAndDrinkCards.Add(new EatAndDrinkCard
                {
                    EntityId = entityId,
                    EntityName = "Restaurant",
                    ImagePath = "image.png",
                    BadgeText = "Badge",
                    Address = "Address"
                });
                setupContext.EatAndDrinkDetails.Add(CreateDetail(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new EatAndDrinkRepository(context);

            var result = await repository.GetFullEntityByIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.Detail, Is.Not.Null);
            NUnitAssert.That(result.Detail!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetFullEntityByIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("90909090-9090-9090-9090-909090909090");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new EatAndDrinkRepository(context);

            var result = await repository.GetFullEntityByIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }
    }
}
