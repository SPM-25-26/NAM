using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemented.MunicipalityEntities
{
    public class ShoppingRepository(ApplicationDbContext context) : Repository<ShoppingCard, Guid>(context), IShoppingRepository
    {
        public async Task<ShoppingCard?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.Set<ShoppingCard>()
                .FirstOrDefaultAsync(c => c.EntityId == entityId, cancellationToken);
        }

        public async Task<IEnumerable<ShoppingCard>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipalityName))
                return [];

            return await context.Set<ShoppingCard>()
                .Where(c => c.Detail != null
                            && c.Detail.MunicipalityData != null
                            && EF.Functions.Like(c.Detail.MunicipalityData.Name, municipalityName))
                .ToListAsync(cancellationToken);
        }

        public async Task<ShoppingCardDetail?> GetDetailByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.Set<ShoppingCardDetail>()
                .Include(d => d.Neighbors)
                    .ThenInclude(n => n.FeatureCard)
                .Include(d => d.NearestCarPark)
                .Include(d => d.Owner)
                .Include(d => d.OpeningHours)
                    .ThenInclude(o => o.AdmissionType)
                .Include(d => d.OpeningHours)
                    .ThenInclude(o => o.TimeInterval)
                .Include(d => d.TemporaryClosure)
                    .ThenInclude(tc => tc.TimeInterval)
                .Include(d => d.Booking)
                    .ThenInclude(b => b.TimeIntervalDto)
                .Include(d => d.MunicipalityData)
                .Include(d => d.AssociatedServices)
                .Include(d => d.Services)
                .Include(d => d.SellingTypicalProducts)
                .AsSplitQuery()
                .FirstOrDefaultAsync(d => d.Identifier == entityId, cancellationToken);
        }

        public async Task<IEnumerable<ShoppingCard>> GetFullEntityListById(string municipalityName, CancellationToken cancellationToken = default)
        {
            var entities = await GetByMunicipalityNameAsync(municipalityName, cancellationToken);
            foreach (var entity in entities)
            {
                entity.Detail = await GetDetailByEntityIdAsync(entity.EntityId, cancellationToken);
            }

            return entities;
        }

        public async Task<ShoppingCard?> GetFullEntityByIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await GetByEntityIdAsync(entityId, cancellationToken);
            if (entity is null)
                return null;

            entity.Detail = await GetDetailByEntityIdAsync(entityId, cancellationToken);
            return entity;
        }
    }
}