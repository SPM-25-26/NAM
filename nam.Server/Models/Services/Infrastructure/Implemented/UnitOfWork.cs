using nam.Server.Data;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Mappers;
using nam.Server.Models.Services.Infrastructure.Interfaces;
using nam.Server.Models.Services.Infrastructure.Repositories.Implemented;
using nam.Server.Models.Services.Infrastructure.Repositories.Implemented.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Implemented
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            ArtCulture = new ArtCultureRepository(_context);
            Article = new ArticleRepository(_context);
            MunicipalityCard = new MunicipalityCardRepository(_context);
            Nature = new NatureRepository(_context);
            Organization = new OrganizationRepository(_context);
            PublicEvent = new PublicEventRepository(_context);
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
        public IArtCultureRepository ArtCulture { get; }
        public IArticleRepository Article { get; }
        public IMunicipalityCardRepository MunicipalityCard { get; }
        public INatureRepository Nature { get; }
        public IOrganizationRepository Organization { get; }
        public IPublicEventRepository PublicEvent { get; }
    }
}
