using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemented.MunicipalityEntities
{
    public class EatAndDrinkRepository(ApplicationDbContext context)
        : Repository<EatAndDrinkCard, Guid>(context), IEatAndDrinkRepository
    {
        public async Task<EatAndDrinkCard?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.Set<EatAndDrinkCard>()
                .FirstOrDefaultAsync(c => c.EntityId == entityId, cancellationToken);
        }

        public async Task<IEnumerable<EatAndDrinkCard>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipalityName))
                return [];

            return await context.Set<EatAndDrinkCard>()
                .Where(c => c.Detail != null
                            && c.Detail.MunicipalityData != null
                            && EF.Functions.Like(c.Detail.MunicipalityData.Name, municipalityName))
                .ToListAsync(cancellationToken);
        }

        public async Task<EatAndDrinkDetail?> GetDetailByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.Set<EatAndDrinkDetail>()
                .Include(d => d.Neighbors)
                    .ThenInclude(n => n.FeatureCard)
                .Include(d => d.NearestCarPark)
                .Include(d => d.Owner)
                .Include(d => d.MunicipalityData)
                .Include(d => d.AssociatedServices)
                .Include(d => d.Services)
                .Include(d => d.TypicalProducts)
                .AsSplitQuery()
                .FirstOrDefaultAsync(d => d.Identifier == entityId, cancellationToken);
        }

        public async Task<IEnumerable<EatAndDrinkCard>> GetFullEntityListById(string municipalityName, CancellationToken cancellationToken = default)
        {
            var entities = await GetByMunicipalityNameAsync(municipalityName, cancellationToken);
            foreach (var entity in entities)
            {
                entity.Detail = await GetDetailByEntityIdAsync(entity.EntityId, cancellationToken);
            }

            return entities;
        }

        public async Task<EatAndDrinkCard?> GetFullEntityByIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await GetByEntityIdAsync(entityId, cancellationToken);
            if (entity is null)
                return null;

            entity.Detail = await GetDetailByEntityIdAsync(entityId, cancellationToken);
            return entity;
        }
    }
}