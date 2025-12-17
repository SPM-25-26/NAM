using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Implemented.MunicipalityEntities
{
    public class MunicipalityCardRepository(ApplicationDbContext context) : Repository<MunicipalityCard, string>(context), IMunicipalityCardRepository
    {
        public async Task<MunicipalityCard?> GetByEntityIdAsync(string legalName, CancellationToken cancellationToken = default)
        {
            return await context.MunicipalityCards
                .FirstOrDefaultAsync(c => c.LegalName == legalName, cancellationToken);
        }

        public async Task<IEnumerable<MunicipalityCard>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipalityName))
                return [];

            return await context.MunicipalityCards
                .Where(c => EF.Functions.Like(c.LegalName, $"%{municipalityName}%")).ToListAsync(cancellationToken);
        }

        public async Task<MunicipalityHomeInfo?> GetDetailByEntityIdAsync(string legalName, CancellationToken cancellationToken = default)
        {
            return await context.MunicipalityHomeInfos
                .Include(c => c.Contacts)
                .Include(c => c.Events).ThenInclude(e => e.FeatureCard)
                .Include(c => c.ArticlesAndPaths).ThenInclude(e => e.FeatureCard)
                .FirstOrDefaultAsync(c => c.LegalName == legalName, cancellationToken);
        }
    }
}
