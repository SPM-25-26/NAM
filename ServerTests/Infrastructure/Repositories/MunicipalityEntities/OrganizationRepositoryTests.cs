using Domain.Entities.MunicipalityEntities;
using Infrastructure;
using Infrastructure.Repositories.Implemented.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.Infrastructure.Repositories.MunicipalityEntities
{
    [TestFixture]
    public class OrganizationRepositoryTests
    {
        private static DbContextOptions<ApplicationDbContext> CreateOptions()
        {
            var databaseName = $"OrganizationRepositoryTests_{NUnit.Framework.TestContext.CurrentContext.Test.Name}";
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        private static OrganizationMobileDetail CreateDetail(string taxCode, string municipalityName = "Milano")
        {
            return new OrganizationMobileDetail
            {
                TaxCode = taxCode,
                LegalName = "Organization",
                MunicipalityData = new MunicipalityForLocalStorageSetting { Name = municipalityName }
            };
        }

        private static OrganizationCard CreateCard(string taxCode, string municipalityName = "Milano")
        {
            return new OrganizationCard
            {
                TaxCode = taxCode,
                EntityName = "Organization",
                ImagePath = "image.png",
                BadgeText = "Badge",
                Address = "Address",
                Detail = CreateDetail(taxCode, municipalityName)
            };
        }

        [Test]
        public async Task GetByEntityIdAsync_ReturnsMatchingCard()
        {
            var options = CreateOptions();
            var taxCode = "TAX001";
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.OrganizationCards.Add(CreateCard(taxCode));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new OrganizationRepository(context);

            var result = await repository.GetByEntityIdAsync(taxCode, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.TaxCode, Is.EqualTo(taxCode));
        }

        [Test]
        public async Task GetByEntityIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new OrganizationRepository(context);

            var result = await repository.GetByEntityIdAsync("MISSING", cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetByMunicipalityNameAsync_ReturnsEmpty_WhenBlank()
        {
            var options = CreateOptions();

            await using var context = new ApplicationDbContext(options);
            var repository = new OrganizationRepository(context);

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
                setupContext.OrganizationCards.Add(CreateCard("TAX002", "Milano"));
                setupContext.OrganizationCards.Add(CreateCard("TAX003", "Roma"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new OrganizationRepository(context);

            var result = (await repository.GetByMunicipalityNameAsync("Milano", cancellationToken)).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].TaxCode, Is.EqualTo("TAX002"));
        }

        [Test]
        public async Task GetDetailByEntityIdAsync_ReturnsDetail()
        {
            var options = CreateOptions();
            var taxCode = "TAX004";
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.OrganizationMobileDetails.Add(CreateDetail(taxCode));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new OrganizationRepository(context);

            var result = await repository.GetDetailByEntityIdAsync(taxCode, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.TaxCode, Is.EqualTo(taxCode));
        }

        [Test]
        public async Task GetDetailByEntityIdAsync_ReturnsNull_WhenMissing()
        {
            var options = CreateOptions();
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new OrganizationRepository(context);

            var result = await repository.GetDetailByEntityIdAsync("MISSING", cancellationToken);

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetFullEntityListById_AssignsDetails()
        {
            var options = CreateOptions();
            var taxCode = "TAX005";
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.OrganizationCards.Add(CreateCard(taxCode, "Milano"));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new OrganizationRepository(context);

            var result = (await repository.GetFullEntityListById("Milano", cancellationToken)).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0].Detail, Is.Not.Null);
            NUnitAssert.That(result[0].Detail!.TaxCode, Is.EqualTo(taxCode));
        }

        [Test]
        public async Task GetFullEntityByIdAsync_SetsDetail()
        {
            var options = CreateOptions();
            var taxCode = "TAX006";
            var cancellationToken = CancellationToken.None;

            await using (var setupContext = new ApplicationDbContext(options))
            {
                setupContext.OrganizationCards.Add(new OrganizationCard
                {
                    TaxCode = taxCode,
                    EntityName = "Organization",
                    ImagePath = "image.png",
                    BadgeText = "Badge",
                    Address = "Address"
                });
                setupContext.OrganizationMobileDetails.Add(CreateDetail(taxCode));
                await setupContext.SaveChangesAsync(cancellationToken);
            }

            await using var context = new ApplicationDbContext(options);
            var repository = new OrganizationRepository(context);

            var result = await repository.GetFullEntityByIdAsync(taxCode, cancellationToken);

            NUnitAssert.That(result, Is.Not.Null);
            NUnitAssert.That(result!.Detail, Is.Not.Null);
            NUnitAssert.That(result.Detail!.TaxCode, Is.EqualTo(taxCode));
        }

        [Test]
        public async Task GetFullEntityByIdAsync_Throws_WhenMissing()
        {
            var options = CreateOptions();
            var cancellationToken = CancellationToken.None;

            await using var context = new ApplicationDbContext(options);
            var repository = new OrganizationRepository(context);

            var exception = NUnitAssert.ThrowsAsync<NullReferenceException>(async () =>
                await repository.GetFullEntityByIdAsync("MISSING", cancellationToken));

            NUnitAssert.That(exception, Is.Not.Null);
        }
    }
}
