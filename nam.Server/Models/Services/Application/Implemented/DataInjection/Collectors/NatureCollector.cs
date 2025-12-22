using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Providers;
using nam.Server.Models.Services.Application.Interfaces.DataInjection;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Mappers;
using System.Collections.Concurrent;

namespace nam.Server.Models.Services.Application.Implemented.DataInjection.Collectors
{
    public class NatureCollector : IEntityCollector<Nature>
    {
        private readonly IFetcher _fetcher;
        private readonly BaseProvider<List<ArtCultureNatureCardDto>, List<Nature>> _cardProvider;

        public NatureCollector(IFetcher fetcher)
        {
            _fetcher = fetcher;
            _cardProvider = new(
               fetcher,
               new NatureMapper(),
               "api/nature/card-list",
               new Dictionary<string, string?> { { "municipality", "" } }
           );
        }

        public async Task<List<Nature>> GetEntities(string municipality)
        {
            // 1. Retrieve the master list of Nature cards
            _cardProvider.Query["municipality"] = municipality;
            var natureList = await _cardProvider.GetEntity();

            if (natureList == null || !natureList.Any()) return [];

            var natureBag = new ConcurrentBag<Nature>();

            // 2. Fetch Details in parallel
            await Parallel.ForEachAsync(natureList, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (natureItem, ct) =>
            {
                // Instantiate a local provider to ensure thread safety during parallel execution
                var localDetailProvider = new BaseProvider<ArtCultureNatureDetailDto, ArtCultureNatureDetail>(
                    _fetcher,
                    new ArtCultureCardDetailMapper(),
                    "api/nature/detail/{identifier}",
                    new Dictionary<string, string?> { { "identifier", natureItem.EntityId.ToString() } }
                );

                try
                {
                    var detail = await localDetailProvider.GetEntity();

                    if (detail != null)
                    {
                        // CRITICAL: Align the Detail Identifier with the Parent EntityId.
                        // This prevents ID mismatches during the sync process (Delete/Insert cycle).
                        detail.Identifier = natureItem.EntityId;

                        // Link the fetched detail to the parent entity
                        natureItem.Detail = detail;

                        natureBag.Add(natureItem);
                    }
                    else
                    {
                        // Persist the card even if detail fetch fails to maintain list view integrity
                        natureBag.Add(natureItem);
                    }
                }
                catch (Exception)
                {
                    // Log error if necessary
                }
            });

            return natureBag.ToList();
        }
    }
}