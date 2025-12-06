using nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection;

namespace nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection
{
    public interface ISyncService
    {
        Task ExecuteSyncAsync<TEntity>(IEntityCollector<TEntity> entityCollector) where TEntity : class;
    }
}
