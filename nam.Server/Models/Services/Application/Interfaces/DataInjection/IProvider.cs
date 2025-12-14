namespace nam.Server.Models.Services.Application.Interfaces.DataInjection
{
    public interface IProvider<TEntity>
    {
        Task<TEntity> GetEntity(CancellationToken ct = default);
    }
}
