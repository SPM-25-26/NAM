using nam.Server.Models.Entities.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Services.Interfaces.MunicipalityEntities
{
    public class IArtCultureService : IMunicipalityEntityService<ArtCultureNatureCard, ArtCultureNatureDetail>
    {
        public Task<ArtCultureNatureDetail?> GetCardDetailAsync(string entityId, string language = "it", CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArtCultureNatureCard>> GetCardListAsync(string municipality, string language = "it", CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
