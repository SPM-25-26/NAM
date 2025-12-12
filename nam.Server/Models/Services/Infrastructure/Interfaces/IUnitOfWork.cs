using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces;

namespace nam.Server.Models.Services.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

        Task CompleteAsync();
    }
}
