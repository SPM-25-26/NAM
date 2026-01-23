using DataInjection.Core.Interfaces;
using DataInjection.Core.Providers;
using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.SQL.Collectors
{
    public class MapDataCollector : IEntityCollector<MapData>
    {
        private readonly IFetcher _fetcher;
        private readonly IConfiguration _configuration;
        private readonly ExternalEndpointProvider<MapDataDto, MapData> _provider;

        public MapDataCollector(IFetcher fetcher, IConfiguration configuration)
        {
            _fetcher = fetcher;
            _configuration = configuration;

            _provider = new(
                _configuration,
                _fetcher,
                new MapDataMapper(),
                "api/map",
                new Dictionary<string, string?> { { "municipality", "" } }
            );
        }

        public async Task<List<MapData>> GetEntities(string municipality)
        {
            // La risposta dell’endpoint NON contiene il nome: lo impostiamo dal parametro query
            _provider.Query["municipality"] = municipality;

            var entity = await _provider.GetEntity();
            entity.Name = municipality;

            return [entity];
        }
    }
}