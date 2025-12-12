namespace nam.Server.Models.Services.Application.Interfaces.DataInjection
{
    public interface IEntityCollector<TEntity>
    {
        Task<List<TEntity>> GetEntities(string municipality);
    }
}
