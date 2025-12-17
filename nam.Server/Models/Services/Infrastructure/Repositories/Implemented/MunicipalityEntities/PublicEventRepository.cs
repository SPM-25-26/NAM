using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Implemented.MunicipalityEntities
{
    public class PublicEventRepository(ApplicationDbContext context) : Repository<PublicEventCard, Guid>(context), IPublicEventRepository
    {
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
    }
}
