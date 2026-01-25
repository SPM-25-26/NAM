using Domain.Entities.MunicipalityEntities;
using Infrastructure;
using Infrastructure.Repositories.Implemented.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.Infrastructure.Repositories.MunicipalityEntities
{
    [TestFixture]
    public class ServiceRepositoryTests
    {
        private static DbContextOptions<ApplicationDbContext> CreateOptions()
        {
            var databaseName = $"ServiceRepositoryTests_{NUnit.Framework.TestContext.CurrentContext.Test.Name}";
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        private static ServiceDetail CreateDetail(Guid entityId, string municipalityName = "Milano")
        {
            return new ServiceDetail
            {
                Identifier = entityId,
                Name = "Service",
                MunicipalityData = new MunicipalityForLocalStorageSetting { Name = municipalityName }
            };
        }

        private static ServiceCard CreateCard(Guid entityId, string municipalityName = "Milano")
        {
            return new ServiceCard
            {
                EntityId = entityId,
                EntityName = "Service",
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
            var entityId = Guid.Parse("48484848-4848-4848-4848-484848484848");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.ServiceCards.Add(CreateCard(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new ServiceRepository(context);

            var result = await repository.GetByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.EntityId, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetByEntityIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("49494949-4949-4949-4949-494949494949");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new ServiceRepository(context);

            var result = await repository.GetByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsEmpty_WhenBlank()
        {
            var options = CreateOptions();

            await using var context = new ApplicationDbContext(options);
            var repository = new ServiceRepository(context);

            var result = await repository.GetByMunicipalityNameAsync(" ");

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsMatchingCards()
        {
            var options = CreateOptions();
            var milanoId = Guid.Parse("50505050-5050-5050-5050-505050505051");
            var romaId = Guid.Parse("51515151-5151-5151-5151-515151515151");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.ServiceCards.Add(CreateCard(milanoId, "Milano"));
                setupContext.ServiceCards.Add(CreateCard(romaId, "Roma"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new ServiceRepository(context);

            var result = (await repository.GetByMunicipalityNameAsync("Milano", cancellationToken)).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].EntityId, Is.EqualTo(milanoId));
        }

        [Test]
        public async Task GetDetailByEntityIdAsync_ReturnsDetail()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("52525252-5252-5252-5252-525252525252");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.ServiceDetails.Add(CreateDetail(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new ServiceRepository(context);

            var result = await repository.GetDetailByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetDetailByEntityIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("53535353-5353-5353-5353-535353535353");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new ServiceRepository(context);

            var result = await repository.GetDetailByEntityIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetFullEntityListById_AssignsDetails()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("54545454-5454-5454-5454-545454545454");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.ServiceCards.Add(CreateCard(entityId, "Milano"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new ServiceRepository(context);

            var result = (await repository.GetFullEntityListById("Milano", cancellationToken)).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Detail, Is.Not.Null);
            NUnitAssert.That(result[0].Detail!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetFullEntityByIdAsync_SetsDetail()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("55555555-5555-5555-5555-555555555556");
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.ServiceCards.Add(new ServiceCard
                {
                    EntityId = entityId,
                    EntityName = "Service",
                    ImagePath = "image.png",
                    BadgeText = "Badge",
                    Address = "Address"
                });
                setupContext.ServiceDetails.Add(CreateDetail(entityId));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new ServiceRepository(context);

            var result = await repository.GetFullEntityByIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.Detail, Is.Not.Null);
            NUnitAssert.That(result.Detail!.Identifier, Is.EqualTo(entityId));
        }

        [Test]
        public async Task GetFullEntityByIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var entityId = Guid.Parse("56565656-5656-5656-5656-565656565656");
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new ServiceRepository(context);

            var result = await repository.GetFullEntityByIdAsync(entityId, cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }
    }
}
