using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemented.MunicipalityEntities
{
    public class RouteRepository(ApplicationDbContext context) : Repository<RouteCard, Guid>(context), IRouteRepository
    {
        public async Task<RouteCard?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.Set<RouteCard>()
                .FirstOrDefaultAsync(c => c.EntityId == entityId, cancellationToken);
        }

        public async Task<IEnumerable<RouteCard>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipalityName))
                return [];

            return await context.Set<RouteCard>()
                .Where(c => c.Detail != null
                            && c.Detail.MunicipalityData != null
                            && EF.Functions.Like(c.Detail.MunicipalityData.Name, municipalityName))
                .ToListAsync(cancellationToken);
        }

        public async Task<RouteDetail?> GetDetailByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.Set<RouteDetail>()
                .Include(d => d.MunicipalityData)
                .Include(d => d.Stages)
                    .ThenInclude(s => s.StageMobile)
                .Include(d => d.StagesPoi)
                    .ThenInclude(p => p.FeatureCard)
                .AsSplitQuery()
                .FirstOrDefaultAsync(d => d.Identifier == entityId, cancellationToken);
        }

        public async Task<IEnumerable<RouteCard>> GetFullEntityListById(string municipalityName, CancellationToken cancellationToken = default)
        {
            var entities = await GetByMunicipalityNameAsync(municipalityName, cancellationToken);
            foreach (var entity in entities)
            {
                entity.Detail = await GetDetailByEntityIdAsync(entity.EntityId, cancellationToken);
            }

            return entities;
        }

        public async Task<RouteCard?> GetFullEntityByIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await GetByEntityIdAsync(entityId, cancellationToken);
            if (entity is null)
                return null;

            entity.Detail = await GetDetailByEntityIdAsync(entityId, cancellationToken);
            return entity;
        }
    }
}
