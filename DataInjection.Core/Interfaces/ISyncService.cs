namespace DataInjection.Interfaces
{
    public interface ISyncService
    {
        Task ExecuteSyncAsync<TEntity>(IEntityCollector<TEntity> entityCollector);
    }
}
