namespace DataInjection.Interfaces
{
    public interface IEntityCollector<TEntity>
    {
        Task<List<TEntity>> GetEntities(string municipality);
    }
}
