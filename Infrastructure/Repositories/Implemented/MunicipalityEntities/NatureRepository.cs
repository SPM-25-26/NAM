using Domain.Entities.MunicipalityEntities;
using Infrastructure.Extensions;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemented.MunicipalityEntities
{

    public class NatureRepository(ApplicationDbContext context) : Repository<Nature, Guid>(context), INatureRepository, IEntitySource
    {
        public string EntityName => "/api/nature/card";
        //public string EntityName => "nature";
        public async Task<Nature?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.Natures
               .FirstOrDefaultAsync(c => c.EntityId == entityId, cancellationToken);
        }

        public async Task<IEnumerable<Nature>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipalityName))
                return [];

            return await context.Natures
                .Where(c => c.Detail != null
                            && c.Detail.MunicipalityData != null
                            && EF.Functions.Like(c.Detail.MunicipalityData.Name, municipalityName))
                .ToListAsync(cancellationToken);
        }

        public async Task<ArtCultureNatureDetail?> GetDetailByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.ArtCultureNatureDetails
                .Include(c => c.Services)
                .Include(c => c.CulturalProjects)
                .Include(c => c.Catalogues)
                .Include(c => c.CreativeWorks)
                //.Include(c => c.Gallery)
                //.Include(c => c.VirtualTours)
                .Include(c => c.Neighbors)
                    .ThenInclude(n => n.FeatureCard)
                .Include(c => c.AssociatedServices)
                .Include(c => c.NearestCarPark)
                .Include(c => c.Site)
                .Include(c => c.MunicipalityData)
                .FirstOrDefaultAsync(c => c.Identifier == entityId, cancellationToken);
        }

        public async Task<IEnumerable<Nature>> GetFullEntityListById(string municipalityName, CancellationToken cancellationToken = default)
        {
            var entities = await GetByMunicipalityNameAsync(municipalityName);
            foreach (var entity in entities)
            {
                entity.Detail = await GetDetailByEntityIdAsync(entity.EntityId, cancellationToken);
            }
            return entities;
        }

        public async Task<Nature?> GetFullEntityByIdAsync(Guid entityId, CancellationToken cancellationToken = default)
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
