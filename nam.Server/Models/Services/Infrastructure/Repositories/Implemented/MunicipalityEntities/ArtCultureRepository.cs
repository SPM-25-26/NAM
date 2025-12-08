using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Implemented.MunicipalityEntities
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

        public async Task<ArtCultureNatureDetail?> GetDetailByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.ArtCultureNatureDetails
                .Include(c => c.Services)
                .Include(c => c.CulturalProjects)
                .Include(c => c.Catalogues)
                .Include(c => c.CreativeWorks)
                //.Include(c => c.Gallery)
                //.Include(c => c.VirtualTours)
                .Include(c => c.Neighbors)
                .Include(c => c.AssociatedServices)
                .Include(c => c.NearestCarPark)
                .Include(c => c.Site)
                .Include(c => c.MunicipalityData)
                .FirstOrDefaultAsync(c => c.Identifier == entityId, cancellationToken);
        }
    }
}
