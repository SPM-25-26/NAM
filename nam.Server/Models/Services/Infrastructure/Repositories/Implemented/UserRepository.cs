using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.Entities;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Implemented
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

        public async Task<bool> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await AppContext.Set<User>().AddAsync(user, cancellationToken);
            var result = await AppContext.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            AppContext.Set<User>().Update(user);
            var result = await AppContext.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}
