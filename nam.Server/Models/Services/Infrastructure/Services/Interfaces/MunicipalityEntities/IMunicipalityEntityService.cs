namespace nam.Server.Models.Services.Infrastructure.Services.Interfaces.MunicipalityEntities
{
    public interface IMunicipalityEntityService<TEntity, TDetail>
    {
        Task<IEnumerable<TEntity>> GetCardListAsync(string municipality, string language = "it", CancellationToken cancellationToken = default);

        Task<TDetail?> GetCardDetailAsync(string entityId, string language = "it", CancellationToken cancellationToken = default);
    }
}
