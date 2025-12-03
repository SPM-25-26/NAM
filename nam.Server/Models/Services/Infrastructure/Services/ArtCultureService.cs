using nam.Server.Models.Entities;
using nam.Server.Models.Services.Infrastructure.Repositories;

namespace nam.Server.Models.Services.Infrastructure.Services
{
    public class ArtCultureService(IArtCultureRepository artCultureRepository) : IArtCultureService
    {

        public async Task<IEnumerable<ArtCultureNatureCard>> GetCardListAsync(string municipality, string language = "it", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipality) || string.IsNullOrWhiteSpace(language))
                return [];

            return await artCultureRepository.GetByMunicipalityNameAsync(municipality, cancellationToken);
        }

        public async Task<ArtCultureNatureDetail?> GetCardDetailAsync(string entityId, string language = "it", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(entityId))
                return null;

            return await artCultureRepository.GetDetailByEntityIdAsync(entityId, cancellationToken);
        }
    }
}
