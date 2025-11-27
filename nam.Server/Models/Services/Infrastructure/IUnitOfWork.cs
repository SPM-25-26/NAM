using nam.Server.Models.Services.Infrastructure.Repositories;

namespace nam.Server.Models.Services.Infrastructure
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        Task CompleteAsync();
    }
}
