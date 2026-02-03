using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemented.MunicipalityEntities
{
    public class SleepRepository(ApplicationDbContext context) : Repository<SleepCard, Guid>(context), ISleepRepository
    {
        public async Task<SleepCard?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.Set<SleepCard>()
                .FirstOrDefaultAsync(c => c.EntityId == entityId, cancellationToken);
        }

        public async Task<IEnumerable<SleepCard>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipalityName))
                return [];

            return await context.Set<SleepCard>()
                .Where(c => c.Detail != null
                            && c.Detail.MunicipalityData != null
                            && EF.Functions.Like(c.Detail.MunicipalityData.Name, municipalityName))
                .ToListAsync(cancellationToken);
        }

        public async Task<SleepCardDetail?> GetDetailByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.Set<SleepCardDetail>()
                .Include(d => d.Neighbors)
                    .ThenInclude(n => n.FeatureCard)
                .Include(d => d.NearestCarPark)
                .Include(d => d.Owner)
                .Include(d => d.MunicipalityData)
                .Include(d => d.AssociatedServices)
                .Include(d => d.Offers)
                .AsSplitQuery()
                .FirstOrDefaultAsync(d => d.Identifier == entityId, cancellationToken);
        }

        public async Task<IEnumerable<SleepCard>> GetFullEntityListById(string municipalityName, CancellationToken cancellationToken = default)
        {
            var entities = await GetByMunicipalityNameAsync(municipalityName, cancellationToken);
            foreach (var entity in entities)
            {
                entity.Detail = await GetDetailByEntityIdAsync(entity.EntityId, cancellationToken);
            }

            return entities;
        }

        public async Task<SleepCard?> GetFullEntityByIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await GetByEntityIdAsync(entityId, cancellationToken);
            if (entity is null)
                return null;

            entity.Detail = await GetDetailByEntityIdAsync(entityId, cancellationToken);
            return entity;
        }
    }
}