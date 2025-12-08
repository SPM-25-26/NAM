using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.MunicipalityEntities
{
    public abstract class MunicipalityEntityService<TEntity, TDetail>(IMunicipalityEntityRepository<TEntity, TDetail, Guid> repository)
        : IMunicipalityEntityService<TEntity, TDetail>
        where TEntity : class
        where TDetail : class
    {

        public async Task<TDetail?> GetCardDetailAsync(string entityId, string language = "it", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(entityId) || string.IsNullOrWhiteSpace(language))
                return default;
            var entityGuid = Guid.Parse(entityId);
            return await repository.GetDetailByEntityIdAsync(entityGuid, cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetCardListAsync(string municipality, string language = "it", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipality) || string.IsNullOrWhiteSpace(language))
                return [];

            return await repository.GetByMunicipalityNameAsync(municipality, cancellationToken);
        }
    }
}
