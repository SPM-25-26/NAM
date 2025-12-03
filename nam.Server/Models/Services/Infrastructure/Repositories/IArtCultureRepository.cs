using nam.Server.Models.Entities;

namespace nam.Server.Models.Services.Infrastructure.Repositories
{
    public interface IArtCultureRepository : IRepository<ArtCultureNatureCard, Guid>
    {
        Task<ArtCultureNatureCard?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ArtCultureNatureCard>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default);
        Task<ArtCultureNatureDetail?> GetDetailByEntityIdAsync(string entityId, CancellationToken cancellationToken = default);
    }
}
