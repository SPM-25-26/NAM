using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Implemented.MunicipalityEntities
{
    public class EntertainmentLeisureRepository(ApplicationDbContext context) : Repository<EntertainmentLeisureCard, Guid>(context), IEntertainmentLeisureRepository
    {
        public async Task<EntertainmentLeisureCard?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.EntertainmentLeisureCards
                .FirstOrDefaultAsync(c => c.EntityId == entityId, cancellationToken);
        }

        public async Task<IEnumerable<EntertainmentLeisureCard>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipalityName))
                return [];

            return await context.EntertainmentLeisureCards
                .Where(c => c.Detail != null
                            && c.Detail.MunicipalityData != null
                            && EF.Functions.Like(c.Detail.MunicipalityData.Name, municipalityName))
                .ToListAsync(cancellationToken);
        }

        public async Task<EntertainmentLeisureDetail?> GetDetailByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.EntertainmentLeisureDetails
               .Include(c => c.Neighbors)
               .ThenInclude(n => n.FeatureCard)
               .Include(c => c.NearestCarPark)
               .Include(c => c.AssociatedServices)
               .Include(c => c.MunicipalityData)
               .AsSplitQuery()
               .FirstOrDefaultAsync(c => c.Identifier == entityId, cancellationToken);
        }
    }
}
