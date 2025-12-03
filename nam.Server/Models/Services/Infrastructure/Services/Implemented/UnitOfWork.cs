using nam.Server.Data;
using nam.Server.Models.Services.Infrastructure.Repositories;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
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
    }
}
