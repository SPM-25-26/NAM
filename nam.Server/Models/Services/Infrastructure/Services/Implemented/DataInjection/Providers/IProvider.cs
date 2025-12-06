namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Providers
{
    public interface IProvider<TEntity>
    {
        Task<TEntity> GetEntity(CancellationToken ct = default);
    }
}
