using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Implemented.MunicipalityEntities
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
                .Include(c => c.Neighbors)
                .Include(c => c.NearestCarPark)
                .Include(c => c.OwnedPoi)
                .Include(c => c.Offers)
                .Include(c => c.Events)
                .Include(c => c.MunicipalityData)
                .FirstOrDefaultAsync(c => c.TaxCode == entityId, cancellationToken);
        }
    }
}
