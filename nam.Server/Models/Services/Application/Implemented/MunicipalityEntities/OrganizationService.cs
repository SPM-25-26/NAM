using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Interfaces.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Interfaces;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Application.Implemented.MunicipalityEntities
{
    public class OrganizationService(IUnitOfWork unitOfWork) : IMunicipalityEntityService<OrganizationCard, OrganizationMobileDetail>
    {
        private readonly IOrganizationRepository organizationRepository = unitOfWork.Organization;
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
