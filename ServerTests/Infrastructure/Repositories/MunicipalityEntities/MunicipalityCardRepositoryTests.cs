using Domain.Entities.MunicipalityEntities;
using Infrastructure;
using Infrastructure.Repositories.Implemented.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.Infrastructure.Repositories.MunicipalityEntities
{
    [TestFixture]
    public class MunicipalityCardRepositoryTests
    {
        private static DbContextOptions<ApplicationDbContext> CreateOptions()
        {
            var databaseName = $"MunicipalityCardTests_{NUnit.Framework.TestContext.CurrentContext.Test.Name}";
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        private static MunicipalityCard CreateCard(string legalName, string imagePath = "card.png")
        {
            return new MunicipalityCard
            {
                LegalName = legalName,
                ImagePath = imagePath
            };
        }

        private static MunicipalityHomeInfo CreateHomeInfo(string legalName, string name = "Name")
        {
            return new MunicipalityHomeInfo
            {
                LegalName = legalName,
                Name = name,
                Description = "Description",
                LogoPath = "logo.png",
                Latitude = 1.2,
                Longitude = 3.4,
                NameAndProvince = "Name (PR)"
            };
        }

        [Test]
        public async Task GetByEntityIdAsync_ReturnsMatchingCard()
        {
            var options = CreateOptions();
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.MunicipalityCards.Add(CreateCard("Milano"));
                setupContext.MunicipalityCards.Add(CreateCard("Roma"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new MunicipalityCardRepository(context);

            var result = await repository.GetByEntityIdAsync("Roma", cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.LegalName, Is.EqualTo("Roma"));
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsEmpty_WhenNameIsBlank()
        {
            var options = CreateOptions();

            await using var context = new ApplicationDbContext(options);
            var repository = new MunicipalityCardRepository(context);

            var result = await repository.GetByMunicipalityNameAsync(" ");

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsMatchingCards()
        {
            var options = CreateOptions();
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.MunicipalityCards.Add(CreateCard("Comune di Milano"));
                setupContext.MunicipalityCards.Add(CreateCard("Comune di Roma"));
                setupContext.MunicipalityCards.Add(CreateCard("Torino"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new MunicipalityCardRepository(context);

            var result = (await repository.GetByMunicipalityNameAsync("Comune", cancellationToken)).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(2));
            NUnitAssert.That(result.Select(card => card.LegalName), Is.EquivalentTo(new[] { "Comune di Milano", "Comune di Roma" }));
        }

        [Test]
        public async Task GetDetailByEntityIdAsync_ReturnsMatchingHomeInfo()
        {
            var options = CreateOptions();
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                var info = CreateHomeInfo("Milano", "Milano");
                info.Contacts = new MunicipalityHomeContactInfo { Email = "info@milano.it" };
                setupContext.MunicipalityHomeInfos.Add(info);
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new MunicipalityCardRepository(context);

            var result = await repository.GetDetailByEntityIdAsync("Milano", cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.LegalName, Is.EqualTo("Milano"));
            NUnitAssert.That(result.Contacts, Is.Not.Null);
            NUnitAssert.That(result.Contacts!.Email, Is.EqualTo("info@milano.it"));
        }

        [Test]
        public async Task GetFullEntityListById_AssignsDetails()
        {
            var options = CreateOptions();
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.MunicipalityCards.Add(CreateCard("Comune di Milano"));
                setupContext.MunicipalityCards.Add(CreateCard("Comune di Roma"));
                setupContext.MunicipalityHomeInfos.Add(CreateHomeInfo("Comune di Milano", "Milano"));
                setupContext.MunicipalityHomeInfos.Add(CreateHomeInfo("Comune di Roma", "Roma"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new MunicipalityCardRepository(context);

            var result = (await repository.GetFullEntityListById("Comune", cancellationToken)).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(2));
            NUnitAssert.That(result.All(card => card.Detail is not null), Is.True);
            NUnitAssert.That(result.Select(card => card.Detail!.LegalName), Is.EquivalentTo(new[] { "Comune di Milano", "Comune di Roma" }));
        }

        [Test]
        public async Task GetFullEntityByIdAsync_SetsDetailOnEntity()
        {
            var options = CreateOptions();
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.MunicipalityCards.Add(CreateCard("Milano"));
                setupContext.MunicipalityHomeInfos.Add(CreateHomeInfo("Milano", "Milano"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new MunicipalityCardRepository(context);

            var result = await repository.GetFullEntityByIdAsync("Milano", cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.Detail, Is.Not.Null);
            NUnitAssert.That(result.Detail!.LegalName, Is.EqualTo("Milano"));
        }

        [Test]
        public async Task GetFullEntityByIdAsync_Throws_WhenEntityMissing()
        {
            var options = CreateOptions();
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new MunicipalityCardRepository(context);

            var exception = NUnitAssert.ThrowsAsync<NullReferenceException>(async () =>
                await repository.GetFullEntityByIdAsync("Missing", cancellationToken));

            NUnitAssert.That(exception, Is.Not.Null);
        }
    }
}
