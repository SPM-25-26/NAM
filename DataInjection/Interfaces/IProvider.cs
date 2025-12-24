namespace DataInjection.Interfaces
{
    public interface IProvider<TEntity>
    {
        Task<TEntity> GetEntity(CancellationToken ct = default);
    }
}
