using Domain.Entities;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemented
{
    public class UserRepository(ApplicationDbContext context) : Repository<User, Guid>(context), IUserRepository
    {
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
