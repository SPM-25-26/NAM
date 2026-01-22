using Infrastructure.Repositories.Implemented;
using Infrastructure.Repositories.Implemented.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Questionaires = new UserRepository(_context);
            ArtCulture = new ArtCultureRepository(_context);
            Article = new ArticleRepository(_context);
            MunicipalityCard = new MunicipalityCardRepository(_context);
            Nature = new NatureRepository(_context);
            Organization = new OrganizationRepository(_context);
            PublicEvent = new PublicEventRepository(_context);
            EntertainmentLeisure = new EntertainmentLeisureRepository(_context);
            Route = new RouteRepository(_context);
            Service = new ServiceRepository(_context);
            Shopping = new ShoppingRepository(_context);
        }

        public Task CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IUserRepository Users { get; }
        public IUserRepository Questionaires { get; }
        public IArtCultureRepository ArtCulture { get; }
        public IArticleRepository Article { get; }
        public IMunicipalityCardRepository MunicipalityCard { get; }
        public INatureRepository Nature { get; }
        public IOrganizationRepository Organization { get; }
        public IPublicEventRepository PublicEvent { get; }
        public IEntertainmentLeisureRepository EntertainmentLeisure { get; }
        public IRouteRepository Route { get; }
        public IServiceRepository Service { get; }

        public IShoppingRepository Shopping { get; }
    }
}
