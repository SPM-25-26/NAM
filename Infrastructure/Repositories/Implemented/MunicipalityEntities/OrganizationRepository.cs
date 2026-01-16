using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemented.MunicipalityEntities
{
    public class OrganizationRepository(ApplicationDbContext context) : Repository<OrganizationCard, string>(context), IOrganizationRepository
    {
        public async Task<OrganizationCard?> GetByEntityIdAsync(string entityId, CancellationToken cancellationToken = default)
        {
            return await context.OrganizationCards
                .FirstOrDefaultAsync(c => c.TaxCode == entityId, cancellationToken);
        }

        public async Task<IEnumerable<OrganizationCard>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipalityName))
                return [];

            return await context.OrganizationCards
                .Where(c => c.Detail != null
                            && c.Detail.MunicipalityData != null
                            && EF.Functions.Like(c.Detail.MunicipalityData.Name, municipalityName))
                .ToListAsync(cancellationToken);
        }

        public async Task<OrganizationMobileDetail?> GetDetailByEntityIdAsync(string entityId, CancellationToken cancellationToken = default)
        {
            return await context.OrganizationMobileDetails
                .Include(c => c.Neighbors).ThenInclude(n => n.FeatureCard)
                .Include(c => c.NearestCarPark)
                .Include(c => c.OwnedPoi)
                .Include(c => c.Offers)
                .Include(c => c.Events)
                .Include(c => c.MunicipalityData)
                .FirstOrDefaultAsync(c => c.TaxCode == entityId, cancellationToken);
        }

        public async Task<IEnumerable<OrganizationCard>> GetFullEntityListById(string municipalityName, CancellationToken cancellationToken = default)
        {
            var entities = await GetByMunicipalityNameAsync(municipalityName);
            foreach (var entity in entities)
            {
                entity.Detail = await GetDetailByEntityIdAsync(entity.TaxCode, cancellationToken);
            }
            return entities;
        }

        public async Task<OrganizationCard?> GetFullEntityByIdAsync(string entityId, CancellationToken cancellationToken = default)
        {
            var entity = await GetByEntityIdAsync(entityId, cancellationToken);
            entity.Detail = await GetDetailByEntityIdAsync(entityId, cancellationToken);
            return entity;
        }
    }
}
