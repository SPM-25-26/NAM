using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Infrastructure.UnitOfWork;
using nam.Server.Services.Implemented.MunicipalityEntities;
using nam.Server.Services.Interfaces.MunicipalityEntities;
using NSubstitute;
using NUnit.Framework;
using NUnitAssert = NUnit.Framework.Assert;

namespace nam.ServerTests.NamServer.MunicipalityEntities
{
    [TestFixture]
    public class MunicipalityEntityGuidServicesTests
    {
        [Test]
        public async Task ArtCultureService_GetCardListAsync_ReturnsRepositoryCards()
        {
            var entity = new ArtCultureNatureCard { EntityName = "Culture" };
            await VerifyGuidServiceGetCardListAsync<ArtCultureNatureCard, ArtCultureNatureDetail, IArtCultureRepository>(
                unitOfWork => new ArtCultureService(unitOfWork),
                (unitOfWork, repository) => unitOfWork.ArtCulture.Returns(repository),
                entity);
        }

        [Test]
        public async Task ArticleService_GetCardListAsync_ReturnsRepositoryCards()
        {
            var entity = new ArticleCard
            {
                EntityId = Guid.NewGuid(),
                EntityName = "Article",
                BadgeText = "News",
                ImagePath = "image.png"
            };
            await VerifyGuidServiceGetCardListAsync<ArticleCard, ArticleDetail, IArticleRepository>(
                unitOfWork => new ArticleService(unitOfWork),
                (unitOfWork, repository) => unitOfWork.Article.Returns(repository),
                entity);
        }

        [Test]
        public async Task EatAndDrinkService_GetCardListAsync_ReturnsRepositoryCards()
        {
            var entity = new EatAndDrinkCard { EntityName = "Cafe" };
            await VerifyGuidServiceGetCardListAsync<EatAndDrinkCard, EatAndDrinkDetail, IEatAndDrinkRepository>(
                unitOfWork => new EatAndDrinkService(unitOfWork),
                (unitOfWork, repository) => unitOfWork.EatAndDrink.Returns(repository),
                entity);
        }

        [Test]
        public async Task EntertainmentLeisureService_GetCardListAsync_ReturnsRepositoryCards()
        {
            var entity = new EntertainmentLeisureCard { EntityName = "Cinema" };
            await VerifyGuidServiceGetCardListAsync<EntertainmentLeisureCard, EntertainmentLeisureDetail, IEntertainmentLeisureRepository>(
                unitOfWork => new EntertainmentLeisureService(unitOfWork),
                (unitOfWork, repository) => unitOfWork.EntertainmentLeisure.Returns(repository),
                entity);
        }

        [Test]
        public async Task NatureService_GetCardListAsync_ReturnsRepositoryCards()
        {
            var entity = new Nature { EntityName = "Park" };
            await VerifyGuidServiceGetCardListAsync<Nature, ArtCultureNatureDetail, INatureRepository>(
                unitOfWork => new NatureService(unitOfWork),
                (unitOfWork, repository) => unitOfWork.Nature.Returns(repository),
                entity);
        }

        [Test]
        public async Task PublicEventService_GetCardListAsync_ReturnsRepositoryCards()
        {
            var entity = new PublicEventCard
            {
                EntityId = Guid.NewGuid(),
                EntityName = "Event",
                ImagePath = "image.png",
                BadgeText = "Festival",
                Address = "Main Square",
                Date = "2024-01-01"
            };
            await VerifyGuidServiceGetCardListAsync<PublicEventCard, PublicEventMobileDetail, IPublicEventRepository>(
                unitOfWork => new PublicEventService(unitOfWork),
                (unitOfWork, repository) => unitOfWork.PublicEvent.Returns(repository),
                entity);
        }

        [Test]
        public async Task RouteService_GetCardListAsync_ReturnsRepositoryCards()
        {
            var entity = new RouteCard { EntityName = "Trail" };
            await VerifyGuidServiceGetCardListAsync<RouteCard, RouteDetail, IRouteRepository>(
                unitOfWork => new RouteService(unitOfWork),
                (unitOfWork, repository) => unitOfWork.Route.Returns(repository),
                entity);
        }

        [Test]
        public async Task ServiceService_GetCardListAsync_ReturnsRepositoryCards()
        {
            var entity = new ServiceCard { EntityName = "Parking" };
            await VerifyGuidServiceGetCardListAsync<ServiceCard, ServiceDetail, IServiceRepository>(
                unitOfWork => new ServiceService(unitOfWork),
                (unitOfWork, repository) => unitOfWork.Service.Returns(repository),
                entity);
        }

        [Test]
        public async Task ShoppingService_GetCardListAsync_ReturnsRepositoryCards()
        {
            var entity = new ShoppingCard { EntityName = "Market" };
            await VerifyGuidServiceGetCardListAsync<ShoppingCard, ShoppingCardDetail, IShoppingRepository>(
                unitOfWork => new ShoppingService(unitOfWork),
                (unitOfWork, repository) => unitOfWork.Shopping.Returns(repository),
                entity);
        }

        [Test]
        public async Task SleepService_GetCardListAsync_ReturnsRepositoryCards()
        {
            var entity = new SleepCard { EntityName = "Hotel" };
            await VerifyGuidServiceGetCardListAsync<SleepCard, SleepCardDetail, ISleepRepository>(
                unitOfWork => new SleepService(unitOfWork),
                (unitOfWork, repository) => unitOfWork.Sleep.Returns(repository),
                entity);
        }

        private static async Task VerifyGuidServiceGetCardListAsync<TEntity, TDetail, TRepository>(
            Func<IUnitOfWork, IMunicipalityEntityService<TEntity, TDetail>> serviceFactory,
            Action<IUnitOfWork, TRepository> registerRepository,
            TEntity entity)
            where TRepository : class, IMunicipalityEntityRepository<TEntity, TDetail, Guid>
            where TEntity : class
            where TDetail : class
        {
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var repository = Substitute.For<TRepository>();
            registerRepository(unitOfWork, repository);
            repository.GetByMunicipalityNameAsync("Milano", Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IEnumerable<TEntity>>(new[] { entity }));

            var service = serviceFactory(unitOfWork);

            var result = (await service.GetCardListAsync("Milano")).ToList();

            NUnitAssert.That(result, Has.Count.EqualTo(1));
            NUnitAssert.That(result[0], Is.SameAs(entity));
        }
    }
}
