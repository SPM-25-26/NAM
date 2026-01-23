using Domain.Entities.MunicipalityEntities;

namespace nam.Server.Services.Interfaces.MunicipalityEntities
{
    public interface IMapDataService
    {
        public Task<MapData> GetCardAsync(string municipality, string language = "it", CancellationToken cancellationToken = default);
    }
}
