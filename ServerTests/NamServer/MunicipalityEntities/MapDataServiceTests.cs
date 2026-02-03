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
    public class MapDataServiceTests
    {
        private IUnitOfWork _unitOfWork = null!;
        private IMapDataRepository _repository = null!;
        private MapDataService _service = null!;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _repository = Substitute.For<IMapDataRepository>();
            _unitOfWork.MapData.Returns(_repository);
            _service = new MapDataService(_unitOfWork);
        }

        [Test]
        public async Task GetCardAsync_ReturnsNull_WhenMunicipalityIsBlank()
        {
            var result = await _service.GetCardAsync(" ");

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetCardAsync_ReturnsNull_WhenLanguageIsBlank()
        {
            var result = await _service.GetCardAsync("Milano", " ");

            NUnitAssert.That(result, Is.Null);
        }

        [Test]
        public async Task GetCardAsync_ReturnsRepositoryMapData()
        {
            var expected = new MapData { Name = "Milano" };
            _repository.GetByMunicipalityNameAsync("Milano", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(expected));

            var result = await _service.GetCardAsync("Milano");

            NUnitAssert.That(result, Is.SameAs(expected));
        }
    }
}
