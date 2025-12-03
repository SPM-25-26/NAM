using nam.Server.Models.Entities.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IArtCultureRepository : IRepository<ArtCultureNatureCard, Guid>
    {
        Task<ArtCultureNatureCard?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ArtCultureNatureCard>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default);
        Task<ArtCultureNatureDetail?> GetDetailByEntityIdAsync(string entityId, CancellationToken cancellationToken = default);
    }
}
