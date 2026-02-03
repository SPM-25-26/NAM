
using Domain.Entities.MunicipalityEntities;
using Infrastructure.Extensions;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemented.MunicipalityEntities
{
    public class ArtCultureRepository(ApplicationDbContext context) : Repository<ArtCultureNatureCard, Guid>(context), IArtCultureRepository, IEntitySource
    {
        public string EntityName => "/api/art-culture/card";
        //public string EntityName => "art-culture";

        public async Task<ArtCultureNatureCard?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
        {
            return await context.ArtCultureNatureCards
                .FirstOrDefaultAsync(c => c.EntityId == entityId, cancellationToken);
        }

        public async Task<IEnumerable<ArtCultureNatureCard>> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipalityName))
                return [];

            return await context.ArtCultureNatureCards
                //.Include(c => c.Detail)
                //    .ThenInclude(d => d.MunicipalityData)
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
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Identifier == entityId, cancellationToken);
        }

        public async Task<IEnumerable<ArtCultureNatureCard>> GetFullEntityListById(string municipalityName, CancellationToken cancellationToken = default)
        {
            var entities = await GetByMunicipalityNameAsync(municipalityName);
            foreach (var entity in entities)
            {
                entity.Detail = await GetDetailByEntityIdAsync(entity.EntityId, cancellationToken);
            }
            return entities;
        }

        public async Task<ArtCultureNatureCard?> GetFullEntityByIdAsync(Guid entityId, CancellationToken cancellationToken = default)
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
