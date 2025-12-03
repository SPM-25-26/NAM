using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.Entities;

namespace nam.Server.Models.Services.Infrastructure.Repositories
{
    public class ArtCultureRepository(ApplicationDbContext context) : Repository<ArtCultureNatureCard, Guid>(context), IArtCultureRepository
    {
        public async Task<ArtCultureNatureCard?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.ArtCultureNatureCards
                .FirstOrDefaultAsync(c => c.EntityId == entityId, cancellationToken);
        }

        public async Task<IEnumerable<ArtCultureNatureCard>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipalityName))
                return [];

            return await context.ArtCultureNatureCards
                //.Include(c => c.Detail)
                //    .ThenInclude(d => d.MunicipalityData)
                .Where(c => c.Detail != null
                            && c.Detail.MunicipalityData != null
                            && EF.Functions.Like(c.Detail.MunicipalityData.Name, municipalityName))
                .ToListAsync(cancellationToken);
        }

        public async Task<ArtCultureNatureDetail?> GetDetailByEntityIdAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(entityId))
                return null;

            return await context.ArtCultureNatureDetails
                .FirstOrDefaultAsync(c => c.Identifier.ToString() == entityId, cancellationToken);
        }
    }
}
