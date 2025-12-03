using nam.Server.Models.Entities.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Services.Interfaces.MunicipalityEntities
{
    public interface IArtCultureService
    {

        Task<IEnumerable<ArtCultureNatureCard>> GetCardListAsync(string municipality, string language = "it", CancellationToken cancellationToken = default);

        Task<ArtCultureNatureDetail?> GetCardDetailAsync(string entityId, string language = "it", CancellationToken cancellationToken = default);
    }
}
