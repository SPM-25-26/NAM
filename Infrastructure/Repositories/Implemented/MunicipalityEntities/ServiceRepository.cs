using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemented.MunicipalityEntities
{
    public class ServiceRepository(ApplicationDbContext context) : Repository<ServiceCard, Guid>(context), IServiceRepository
    {
        public async Task<ServiceCard?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.Set<ServiceCard>()
                .FirstOrDefaultAsync(c => c.EntityId == entityId, cancellationToken);
        }

        public async Task<IEnumerable<ServiceCard>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipalityName))
                return [];

            return await context.Set<ServiceCard>()
                .Where(c => c.Detail != null
                            && c.Detail.MunicipalityData != null
                            && EF.Functions.Like(c.Detail.MunicipalityData.Name, municipalityName))
                .ToListAsync(cancellationToken);
        }

        public async Task<ServiceDetail?> GetDetailByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.Set<ServiceDetail>()
                .Include(d => d.Neighbors)
                    .ThenInclude(n => n.FeatureCard)
                .Include(d => d.NearestCarPark)
                .Include(d => d.Locations)
                    .ThenInclude(l => l!.ServiceLocation)
                .Include(d => d.MunicipalityData)
                .Include(d => d.AssociatedServices)
                .AsSplitQuery()
                .FirstOrDefaultAsync(d => d.Identifier == entityId, cancellationToken);
        }

        public async Task<IEnumerable<ServiceCard>> GetFullEntityListById(string municipalityName, CancellationToken cancellationToken = default)
        {
            var entities = await GetByMunicipalityNameAsync(municipalityName, cancellationToken);
            foreach (var entity in entities)
            {
                entity.Detail = await GetDetailByEntityIdAsync(entity.EntityId, cancellationToken);
            }

            return entities;
        }

        public async Task<ServiceCard?> GetFullEntityByIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await GetByEntityIdAsync(entityId, cancellationToken);
            if (entity is null)
                return null;

            entity.Detail = await GetDetailByEntityIdAsync(entityId, cancellationToken);
            return entity;
        }
    }
}