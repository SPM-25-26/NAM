using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Interfaces.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemented.MunicipalityEntities
{
    public class MapDataRepository(ApplicationDbContext context) : Repository<MapData, string>(context), IMapDataRepository
    {
        public async Task<MapData> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipalityName))
                return new MapData { Name = string.Empty };

            var entity = await context.Set<MapData>()
                .Include(m => m.Marker)
                .AsSplitQuery()
                .FirstOrDefaultAsync(m => m.Name == municipalityName, cancellationToken);

            return entity ?? new MapData
            {
                Name = municipalityName,
                CenterLatitude = 0,
                CenterLongitude = 0,
                Marker = []
            };
        }
    }
}