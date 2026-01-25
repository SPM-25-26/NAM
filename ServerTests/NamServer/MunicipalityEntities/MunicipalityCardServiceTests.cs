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
    public class MunicipalityCardServiceTests
    {
        private IUnitOfWork _unitOfWork = null!;
        private IMunicipalityCardRepository _repository = null!;
        private MunicipalityCardService _service = null!;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _repository = Substitute.For<IMunicipalityCardRepository>();
            _unitOfWork.MunicipalityCard.Returns(_repository);
            _service = new MunicipalityCardService(_unitOfWork);
        }

        [Test]
        public async Task GetCardDetailAsync_ReturnsDefault_WhenLegalNameIsBlank()
        {
            _repository.GetDetailByEntityIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<MunicipalityHomeInfo?>(new MunicipalityHomeInfo()));

            var result = await _service.GetCardDetailAsync(" ");

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetCardDetailAsync_ReturnsDefault_WhenLanguageIsBlank()
        {
            _repository.GetDetailByEntityIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<MunicipalityHomeInfo?>(new MunicipalityHomeInfo()));

            var result = await _service.GetCardDetailAsync("Milano", " ");

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetCardDetailAsync_NormalizesMunicipalityName()
        {
            var expected = new MunicipalityHomeInfo { LegalName = "Comune di Milano" };
            _repository.GetDetailByEntityIdAsync("Comune di Milano", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<MunicipalityHomeInfo?>(expected));

            var result = await _service.GetCardDetailAsync("Milano");

            NUnitAssert.That(result, Is.SameAs(expected));
        }

        [Test]
        public async Task GetCardDetailAsync_TrimsMunicipalityName()
        {
            var expected = new MunicipalityHomeInfo { LegalName = "Comune di Milano" };
            _repository.GetDetailByEntityIdAsync("Comune di Milano", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<MunicipalityHomeInfo?>(expected));

            var result = await _service.GetCardDetailAsync("  Milano ");

            NUnitAssert.That(result, Is.SameAs(expected));
        }

        [Test]
        public async Task GetCardDetailAsync_PreservesPrefixWhenPresent()
        {
            var expected = new MunicipalityHomeInfo { LegalName = "Comune di Torino" };
            _repository.GetDetailByEntityIdAsync("Comune di Torino", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<MunicipalityHomeInfo?>(expected));

            var result = await _service.GetCardDetailAsync("Comune di Torino");

            NUnitAssert.That(result, Is.SameAs(expected));
        }

        [Test]
        public async Task GetCardListAsync_ReturnsRepositoryCards()
        {
            var expected = new List<MunicipalityCard>
            {
                new() { LegalName = "Comune di Milano" }
            };
            _repository.GetByMunicipalityNameAsync("Milano", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IEnumerable<MunicipalityCard>>(expected));

            var result = (await _service.GetCardListAsync("Milano")).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0], Is.SameAs(expected[0]));
        }

        [Test]
        public async Task GetFullCardAsync_ReturnsRepositoryCard()
        {
            var expected = new MunicipalityCard { LegalName = "Comune di Milano" };
            _repository.GetFullEntityByIdAsync("Comune di Milano", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<MunicipalityCard?>(expected));

            var result = await _service.GetFullCardAsync("Comune di Milano");

            NUnitAssert.That(result, Is.SameAs(expected));
        }

        [Test]
        public async Task GetFullCardListAsync_ReturnsRepositoryCards()
        {
            var expected = new List<MunicipalityCard>
            {
                new() { LegalName = "Comune di Milano" }
            };
            _repository.GetFullEntityListById("Milano", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IEnumerable<MunicipalityCard>>(expected));

            var result = (await _service.GetFullCardListAsync("Milano")).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0], Is.SameAs(expected[0]));
        }
    }
}
