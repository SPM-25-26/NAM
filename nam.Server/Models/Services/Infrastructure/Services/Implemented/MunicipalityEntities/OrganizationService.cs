using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.MunicipalityEntities
{
    public class OrganizationService(IOrganizationRepository organizationRepository) : IMunicipalityEntityService<OrganizationCard, OrganizationMobileDetail>
    {
        public async Task<OrganizationMobileDetail?> GetCardDetailAsync(string entityId, string language = "it", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(entityId) || string.IsNullOrWhiteSpace(language))
                return default;
            return await organizationRepository.GetDetailByEntityIdAsync(entityId, cancellationToken);

        }

        public async Task<IEnumerable<OrganizationCard>> GetCardListAsync(string municipality, string language = "it", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipality) || string.IsNullOrWhiteSpace(language))
                return [];

            return await organizationRepository.GetByMunicipalityNameAsync(municipality, cancellationToken);
        }
    }
}
