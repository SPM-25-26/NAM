namespace Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IMunicipalityEntityRepository<TEntity, TDetail, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        Task<TEntity?> GetByEntityIdAsync(TKey entityId, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default);
        Task<TDetail?> GetDetailByEntityIdAsync(TKey entityId, CancellationToken cancellationToken = default);

        Task<TEntity?> GetFullEntityByIdAsync(TKey entityId, CancellationToken cancellationToken = default);

    }
}
