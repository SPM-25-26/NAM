using DataInjection.Interfaces;
using DataInjection.Mappers;
using DataInjection.Providers;
using Domain.DTOs.MunicipalityInjection;
using Domain.Entities.MunicipalityEntities;
using System.Collections.Concurrent;

namespace DataInjection.Collectors
{
    public class PublicEventCollector : IEntityCollector<PublicEventCard>
    {
        private readonly IFetcher _fetcher;
        private readonly BaseProvider<List<PublicEventCardDto>, List<PublicEventCard>> _cardProvider;

        public PublicEventCollector(IFetcher fetcher)
        {
            _fetcher = fetcher;
            _cardProvider = new(
               fetcher,
               new PublicEventCardMapper(),
               "api/events/card-list",
               new Dictionary<string, string?> { { "municipality", "" } }
           );
        }

        public async Task<List<PublicEventCard>> GetEntities(string municipality)
        {
            // 1. Retrieve the master list of Public Events
            _cardProvider.Query["municipality"] = municipality;
            var eventList = await _cardProvider.GetEntity();

            if (eventList == null || !eventList.Any()) return [];

            var eventsBag = new ConcurrentBag<PublicEventCard>();

            // 2. Fetch Details in parallel
            await Parallel.ForEachAsync(eventList, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (publicEvent, ct) =>
            {
                // Instantiate a local provider to ensure thread safety during parallel execution
                var localDetailProvider = new BaseProvider<PublicEventMobileDetailDto, PublicEventMobileDetail>(
                    _fetcher,
                    new PublicEventCardDetailMapper(),
                    "api/events/detail/{identifier}",
                    new Dictionary<string, string?> { { "identifier", publicEvent.EntityId.ToString() } }
                );

                try
                {
                    var detail = await localDetailProvider.GetEntity();

                    if (detail != null)
                    {
                        // CRITICAL: Force ID alignment.
                        // Ensure the Detail Identifier matches the Parent EntityId to prevent ID mismatches during sync/cleanup.
                        detail.Identifier = publicEvent.EntityId;

                        // Link the fetched detail to the parent card
                        publicEvent.Detail = detail;

                        eventsBag.Add(publicEvent);
                    }
                    else
                    {
                        // Persist the card even if detail fetch fails
                        eventsBag.Add(publicEvent);
                    }
                }
                catch (Exception)
                {
                    // Log error if necessary
                }
            });

            return eventsBag.ToList();
        }
    }
}