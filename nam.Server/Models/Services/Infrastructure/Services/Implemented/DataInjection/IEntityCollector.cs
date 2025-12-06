namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection
{
    public interface IEntityCollector<TEntity>
    {
        Task<List<TEntity>> GetEntities(string municipality);
    }
}
