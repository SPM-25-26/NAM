namespace nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection
{
    public interface IEntityCollector<TEntity>
    {
        Task<List<TEntity>> GetEntities(string municipality);
    }
}
