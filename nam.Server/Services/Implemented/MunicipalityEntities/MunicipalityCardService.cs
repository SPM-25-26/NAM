using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Infrastructure.UnitOfWork;
using nam.Server.Services.Interfaces.MunicipalityEntities;

namespace nam.Server.Services.Implemented.MunicipalityEntities
{
    public class MunicipalityCardService(IUnitOfWork unitOfWork) : IMunicipalityEntityService<MunicipalityCard, MunicipalityHomeInfo>
    {
        private readonly IMunicipalityCardRepository municipalityCardRepository = unitOfWork.MunicipalityCard;
        public async Task<MunicipalityHomeInfo?> GetCardDetailAsync(string legalName, string language = "it", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(legalName) || string.IsNullOrWhiteSpace(language))
                return default;
            var cleanName = legalName.Trim();
            if (!cleanName.StartsWith("Comune di ", StringComparison.OrdinalIgnoreCase))
            {
                cleanName = $"Comune di {cleanName}";
            }
            return await municipalityCardRepository.GetDetailByEntityIdAsync(cleanName, cancellationToken);
        }

        public async Task<IEnumerable<MunicipalityCard>> GetCardListAsync(string municipality, string language = "it", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipality) || string.IsNullOrWhiteSpace(language))
                return [];

            return await municipalityCardRepository.GetByMunicipalityNameAsync(municipality, cancellationToken);
        }

        public async Task<MunicipalityCard?> GetFullCardAsync(string entityId, string language = "it", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(entityId) || string.IsNullOrWhiteSpace(language))
                return default;
            return await municipalityCardRepository.GetFullEntityByIdAsync(entityId, cancellationToken);
        }

        public async Task<IEnumerable<MunicipalityCard>> GetFullCardListAsync(string municipality, string language = "it", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipality) || string.IsNullOrWhiteSpace(language))
                return [];
            return await municipalityCardRepository.GetFullEntityListById(municipality, cancellationToken);
        }
    }
}
