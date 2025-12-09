using Microsoft.EntityFrameworkCore;
using nam.Server.Data;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Repositories.Implemented.MunicipalityEntities
{
    public class ArticleRepository(ApplicationDbContext context) : Repository<ArticleCard, Guid>(context), IArticleRepository
    {
        public async Task<ArticleCard?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.ArticleCards
    .FirstOrDefaultAsync(c => c.EntityId == entityId, cancellationToken);
        }

        public async Task<IEnumerable<ArticleCard>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipalityName))
                return [];

            return await context.ArticleCards
                //.Include(c => c.Detail)
                //    .ThenInclude(d => d.MunicipalityData)
                .Where(c => c.Detail != null
                            && c.Detail.MunicipalityData != null
                            && EF.Functions.Like(c.Detail.MunicipalityData.Name, municipalityName))
                .ToListAsync(cancellationToken);
        }

        public async Task<ArticleDetail?> GetDetailByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.ArticleDetails
                .Include(d => d.MunicipalityData)
                .Include(d => d.Paragraphs)
                .FirstOrDefaultAsync(c => c.Identifier == entityId, cancellationToken);
        }
    }
}
