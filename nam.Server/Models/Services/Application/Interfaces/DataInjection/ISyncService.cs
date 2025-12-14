namespace nam.Server.Models.Services.Application.Interfaces.DataInjection
{
    public interface ISyncService
    {
        Task ExecuteSyncAsync<TEntity>(IEntityCollector<TEntity> entityCollector) where TEntity : class;
    }
}
