using nam.Server.Models.Entities;

namespace nam.Server.Models.Services.Infrastructure
{
    public interface IArtCultureService
    {

        Task<IEnumerable<ArtCultureNatureCard>> GetCardListAsync(string municipality, string language = "it", CancellationToken cancellationToken = default);

        Task<ArtCultureNatureDetail?> GetCardDetailAsync(string entityId, string language = "it", CancellationToken cancellationToken = default);
    }
}
