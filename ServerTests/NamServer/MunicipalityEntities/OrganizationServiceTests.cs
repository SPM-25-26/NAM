using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Infrastructure.UnitOfWork;
using nam.Server.Services.Implemented.MunicipalityEntities;
using NSubstitute;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.NamServer.MunicipalityEntities
{
    [TestFixture]
    public class OrganizationServiceTests
    {
        private IUnitOfWork _unitOfWork = null!;
        private IOrganizationRepository _repository = null!;
        private OrganizationService _service = null!;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _repository = Substitute.For<IOrganizationRepository>();
            _unitOfWork.Organization.Returns(_repository);
            _service = new OrganizationService(_unitOfWork);
        }

        [Test]
        public async Task GetCardDetailAsync_ReturnsDefault_WhenEntityIdIsBlank()
        {
            _repository.GetDetailByEntityIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<OrganizationMobileDetail?>(new OrganizationMobileDetail()));

            var result = await _service.GetCardDetailAsync(" ");

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetCardDetailAsync_ReturnsDefault_WhenLanguageIsBlank()
        {
            _repository.GetDetailByEntityIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<OrganizationMobileDetail?>(new OrganizationMobileDetail()));

            var result = await _service.GetCardDetailAsync("ORG-1", " ");

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetCardDetailAsync_ReturnsRepositoryDetail()
        {
            var expected = new OrganizationMobileDetail { TaxCode = "ORG-1" };
            _repository.GetDetailByEntityIdAsync("ORG-1", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<OrganizationMobileDetail?>(expected));

            var result = await _service.GetCardDetailAsync("ORG-1");

            NUnitAssert.That(result, Is.SameAs(expected));
        }

        [Test]
        public async Task GetCardListAsync_ReturnsRepositoryCards()
        {
            var expected = new List<OrganizationCard> { new() { TaxCode = "ORG-1" } };
            _repository.GetByMunicipalityNameAsync("Milano", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IEnumerable<OrganizationCard>>(expected));

            var result = (await _service.GetCardListAsync("Milano")).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0], Is.SameAs(expected[0]));
        }

        [Test]
        public async Task GetFullCardAsync_ReturnsRepositoryCard()
        {
            var expected = new OrganizationCard { TaxCode = "ORG-1" };
            _repository.GetFullEntityByIdAsync("ORG-1", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<OrganizationCard?>(expected));

            var result = await _service.GetFullCardAsync("ORG-1");

            NUnitAssert.That(result, Is.SameAs(expected));
        }

        [Test]
        public async Task GetFullCardListAsync_ReturnsRepositoryCards()
        {
            var expected = new List<OrganizationCard> { new() { TaxCode = "ORG-1" } };
            _repository.GetFullEntityListById("Milano", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IEnumerable<OrganizationCard>>(expected));

            var result = (await _service.GetFullCardListAsync("Milano")).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0], Is.SameAs(expected[0]));
        }
    }
}
