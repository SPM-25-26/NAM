
using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemented.MunicipalityEntities
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
                    .ThenInclude(n => n.FeatureCard)
                .Include(c => c.AssociatedServices)
                .Include(c => c.NearestCarPark)
                .Include(c => c.Site)
                .Include(c => c.MunicipalityData)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Identifier == entityId, cancellationToken);
        }

        public async Task<ArtCultureNatureCard?> GetFullEntityByIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await GetByEntityIdAsync(entityId, cancellationToken);
            entity.Detail = await GetDetailByEntityIdAsync(entityId, cancellationToken);
            return entity;
        }
    }
}
