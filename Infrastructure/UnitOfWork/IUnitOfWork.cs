using Infrastructure.Repositories.Interfaces;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IArtCultureRepository ArtCulture { get; }
        IArticleRepository Article { get; }
        IMunicipalityCardRepository MunicipalityCard { get; }
        INatureRepository Nature { get; }
        IOrganizationRepository Organization { get; }
        IPublicEventRepository PublicEvent { get; }
        IEntertainmentLeisureRepository EntertainmentLeisure { get; }
        IRouteRepository Route { get; }
        IServiceRepository Service { get; }
        IShoppingRepository Shopping { get; }
        ISleepRepository Sleep { get; }
        IEatAndDrinkRepository EatAndDrink { get; }

        Task CompleteAsync();
    }
}
