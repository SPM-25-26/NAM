using Domain.Entities.MunicipalityEntities;
using Infrastructure.Repositories.Implemented;
using Infrastructure.UnitOfWork;
using nam.Server.Services.Interfaces.MunicipalityEntities;

namespace nam.Server.Services.Implemented.MunicipalityEntities
{
    public class MapDataService(IUnitOfWork unitOfWork) : IMapDataService
    {
        public async Task<MapData> GetCardAsync(string municipality, string language = "it", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(municipality) || string.IsNullOrWhiteSpace(language))
                return null;

            return await unitOfWork.MapData.GetByMunicipalityNameAsync(municipality, cancellationToken);
        }
    }
}
