namespace nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection
{
    public interface IProvider<TEntity>
    {
        Task<TEntity> GetEntity(CancellationToken ct = default);
    }
}
