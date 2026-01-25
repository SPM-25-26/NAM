using Domain.Entities.MunicipalityEntities;
using Infrastructure.Extensions;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemented.MunicipalityEntities
{
    public class MunicipalityCardRepository(ApplicationDbContext context) : Repository<MunicipalityCard, string>(context), IMunicipalityCardRepository, IEntitySource
    {
        public string EntityName => "municipality";

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
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.LegalName == legalName, cancellationToken);
        }

        public async Task<IEnumerable<MunicipalityCard>> GetFullEntityListById(string municipalityName, CancellationToken cancellationToken = default)
        {
            var entities = await GetByMunicipalityNameAsync(municipalityName);
            foreach (var entity in entities)
            {
                entity.Detail = await GetDetailByEntityIdAsync(entity.LegalName, cancellationToken);
            }
            return entities;
        }

        public async Task<MunicipalityCard?> GetFullEntityByIdAsync(string entityId, CancellationToken cancellationToken = default)
        {
            var entity = await GetByEntityIdAsync(entityId, cancellationToken);
            entity.Detail = await GetDetailByEntityIdAsync(entityId, cancellationToken);
            return entity;
        }

        public async Task<string> GetContentAsync(string id, CancellationToken ct = default)
        {
            var result = await GetFullEntityByIdAsync(id, ct);
            return result.ToEmbeddingString();
        }
    }
}
