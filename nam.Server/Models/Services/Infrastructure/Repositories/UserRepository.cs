using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.Entities;

namespace nam.Server.Models.Services.Infrastructure.Repositories
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        private ApplicationDbContext AppContext => (ApplicationDbContext)_context;

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await AppContext.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return await AppContext.Users
                .AnyAsync(u => u.Email == email, cancellationToken);
        }
    }
}
