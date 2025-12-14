using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Interfaces.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Interfaces;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Application.Implemented.MunicipalityEntities
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
