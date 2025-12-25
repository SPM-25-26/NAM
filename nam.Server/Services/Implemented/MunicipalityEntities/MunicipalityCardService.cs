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
            return await municipalityCardRepository.GetDetailByEntityIdAsync(legalName, cancellationToken);
        }

        public async Task<IEnumerable<MunicipalityCard>> GetCardListAsync(string municipality, string language = "it", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipality) || string.IsNullOrWhiteSpace(language))
                return [];

            return await municipalityCardRepository.GetByMunicipalityNameAsync(municipality, cancellationToken);
        }
    }
}
