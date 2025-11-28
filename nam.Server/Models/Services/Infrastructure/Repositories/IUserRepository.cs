using nam.Server.Models.Entities;

namespace nam.Server.Models.Services.Infrastructure.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> AddAsync(User user, CancellationToken cancellationToken = default);    }
}
