using Domain.Entities.MunicipalityEntities;
using Infrastructure.Extensions;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemented.MunicipalityEntities
{
    public class PublicEventRepository(ApplicationDbContext context) : Repository<PublicEventCard, Guid>(context), IPublicEventRepository, IEntitySource
    {
        public string EntityName => "/api/public-event/card";
        //public string EntityName => "public-event";

        public async Task<PublicEventCard?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.PublicEventCards
               .FirstOrDefaultAsync(c => c.EntityId == entityId, cancellationToken);
        }

        public async Task<IEnumerable<PublicEventCard>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipalityName))
                return [];

            return await context.PublicEventCards
                .Where(c => c.Detail != null
                            && c.Detail.MunicipalityData != null
                            && EF.Functions.Like(c.Detail.MunicipalityData.Name, municipalityName))
                .ToListAsync(cancellationToken);
        }

        public async Task<PublicEventMobileDetail?> GetDetailByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.PublicEventMobileDetails
                .Include(c => c.Neighbors).ThenInclude(n => n.FeatureCard)
                .Include(c => c.NearestCarPark)
                .Include(c => c.Organizer)
                .Include(c => c.TicketsAndCosts)
                .Include(c => c.MunicipalityData)
                .FirstOrDefaultAsync(c => c.Identifier == entityId, cancellationToken);
        }

        public async Task<IEnumerable<PublicEventCard>> GetFullEntityListById(string municipalityName, CancellationToken cancellationToken = default)
        {
            var entities = await GetByMunicipalityNameAsync(municipalityName);
            foreach (var entity in entities)
            {
                entity.Detail = await GetDetailByEntityIdAsync(entity.EntityId, cancellationToken);
            }
            return entities;
        }

        public async Task<PublicEventCard?> GetFullEntityByIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            var entity = await GetByEntityIdAsync(entityId, cancellationToken);
            entity.Detail = await GetDetailByEntityIdAsync(entityId, cancellationToken);
            return entity;
        }

        public async Task<string> GetContentAsync(string id, CancellationToken ct = default)
        {
            var result = await GetFullEntityByIdAsync(Guid.Parse(id), ct);
            return result.ToEmbeddingString();
        }
    }
}
