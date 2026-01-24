using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using nam.Server.Services.Implemented.MunicipalityEntities;
using NSubstitute;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.NamServer.MunicipalityEntities
{
    [TestFixture]
    public class MunicipalityEntityServiceTests
    {
        private IMunicipalityEntityRepository<FakeCard, FakeDetail, Guid> _repository = null!;
        private MunicipalityEntityService<FakeCard, FakeDetail> _service = null!;

        [SetUp]
        public void Setup()
        {
            _repository = Substitute.For<IMunicipalityEntityRepository<FakeCard, FakeDetail, Guid>>();
            _service = new FakeMunicipalityEntityService(_repository);
        }

        [Test]
        public async Task GetCardDetailAsync_ReturnsDefault_WhenEntityIdIsBlank()
        {
            _repository.GetDetailByEntityIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<FakeDetail?>(new FakeDetail()));

            var result = await _service.GetCardDetailAsync(" ");

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetCardDetailAsync_ReturnsDefault_WhenLanguageIsBlank()
        {
            _repository.GetDetailByEntityIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<FakeDetail?>(new FakeDetail()));

            var result = await _service.GetCardDetailAsync("0e910840-4ad7-4aba-8db0-6e36decd75a0", " ");

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetCardDetailAsync_UsesRepositoryForValidEntityId()
        {
            var entityId = Guid.Parse("3d9f2bfa-2b63-4ec1-92cf-2af0f0b3ab8d");
            var expected = new FakeDetail();
            _repository.GetDetailByEntityIdAsync(entityId, Arg.Any<CancellationToken>()).Returns(expected);

            var result = await _service.GetCardDetailAsync(entityId.ToString());

            NUnitAssert.That(result, Is.SameAs(expected));
        }

        [Test]
        public async Task GetCardListAsync_ReturnsEmpty_WhenMunicipalityIsBlank()
        {
            _repository.GetByMunicipalityNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IEnumerable<FakeCard>>(new[] { new FakeCard() }));

            var result = (await _service.GetCardListAsync("\t")).ToList();

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetCardListAsync_ReturnsEmpty_WhenLanguageIsBlank()
        {
            _repository.GetByMunicipalityNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IEnumerable<FakeCard>>(new[] { new FakeCard() }));

            var result = (await _service.GetCardListAsync("Milano", " ")).ToList();

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetCardListAsync_UsesRepositoryForValidMunicipality()
        {
            var expected = new List<FakeCard> { new() };
            _repository.GetByMunicipalityNameAsync("Milano", Arg.Any<CancellationToken>()).Returns(expected);

            var result = (await _service.GetCardListAsync("Milano")).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0], Is.SameAs(expected[0]));
        }

        [Test]
        public async Task GetFullCardAsync_ReturnsDefault_WhenEntityIdIsBlank()
        {
            _repository.GetFullEntityByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<FakeCard?>(new FakeCard()));

            var result = await _service.GetFullCardAsync("");

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetFullCardAsync_ReturnsDefault_WhenLanguageIsBlank()
        {
            _repository.GetFullEntityByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<FakeCard?>(new FakeCard()));

            var result = await _service.GetFullCardAsync("0e910840-4ad7-4aba-8db0-6e36decd75a0", " ");

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetFullCardAsync_UsesRepositoryForValidEntityId()
        {
            var entityId = Guid.Parse("0b51b1a0-5391-4f21-9a0c-f4087e2d487d");
            var expected = new FakeCard();
            _repository.GetFullEntityByIdAsync(entityId, Arg.Any<CancellationToken>()).Returns(expected);

            var result = await _service.GetFullCardAsync(entityId.ToString());

            NUnitAssert.That(result, Is.SameAs(expected));
        }

        [Test]
        public async Task GetFullCardListAsync_ReturnsEmpty_WhenMunicipalityIsBlank()
        {
            _repository.GetFullEntityListById(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IEnumerable<FakeCard>>(new[] { new FakeCard() }));

            var result = (await _service.GetFullCardListAsync(" ")).ToList();

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetFullCardListAsync_ReturnsEmpty_WhenLanguageIsBlank()
        {
            _repository.GetFullEntityListById(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IEnumerable<FakeCard>>(new[] { new FakeCard() }));

            var result = (await _service.GetFullCardListAsync("Milano", " ")).ToList();

            NUnitAssert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetFullCardListAsync_UsesRepositoryForValidMunicipality()
        {
            var expected = new List<FakeCard> { new() };
            _repository.GetFullEntityListById("Milano", Arg.Any<CancellationToken>()).Returns(expected);

            var result = (await _service.GetFullCardListAsync("Milano")).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0], Is.SameAs(expected[0]));
        }

        private sealed class FakeMunicipalityEntityService
            : MunicipalityEntityService<FakeCard, FakeDetail>
        {
            public FakeMunicipalityEntityService(
                IMunicipalityEntityRepository<FakeCard, FakeDetail, Guid> repository)
                : base(repository)
            {
            }
        }
    }

    public class FakeCard
    {
    }

    public class FakeDetail
    {
    }
}
