using DataInjection.DTOs;
using DataInjection.Interfaces;
using DataInjection.Mappers;
using DataInjection.Providers;
using Domain.Entities.MunicipalityEntities;
using System.Collections.Concurrent;

namespace DataInjection.Collectors
{
    public class EntertainmentLeisureCardCollector : IEntityCollector<EntertainmentLeisureCard>
    {
        private readonly IFetcher _fetcher;
        private readonly BaseProvider<List<EntertainmentLeisureCardDto>, List<EntertainmentLeisureCard>> _cardProvider;

        public EntertainmentLeisureCardCollector(IFetcher fetcher)
        {
            _fetcher = fetcher;
            _cardProvider = new(
               fetcher,
               new EntertainmentLeisureCardMapper(),
               "api/entertainment-leisure/card-list",
               new Dictionary<string, string?> { { "municipality", "" } }
           );
        }

        public async Task<List<EntertainmentLeisureCard>> GetEntities(string municipality)
        {
            // 1. Retrieve the master list of Entertainment cards
            _cardProvider.Query["municipality"] = municipality;
            var cards = await _cardProvider.GetEntity();

            if (cards == null || !cards.Any()) return [];

            var cardsBag = new ConcurrentBag<EntertainmentLeisureCard>();

            // 2. Fetch Details in parallel
            await Parallel.ForEachAsync(cards, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (card, ct) =>
            {
                // Instantiate a local provider to ensure thread safety during parallel execution
                var localDetailProvider = new BaseProvider<EntertainmentLeisureDetailDto, EntertainmentLeisureDetail>(
                    _fetcher,
                    new EntertainmentLeisureDetailMapper(),
                    "api/entertainment-leisure/detail/{identifier}",
                    new Dictionary<string, string?> { { "identifier", card.EntityId.ToString() } }
                );

                try
                {
                    var detail = await localDetailProvider.GetEntity();

                    if (detail != null)
                    {
                        // CRITICAL: Force the Detail Identifier to match the Card EntityId.
                        // This prevents ID mismatches during the sync process (Delete/Insert cycle).
                        detail.Identifier = card.EntityId;

                        // Link the fetched detail to the parent card
                        card.Detail = detail;

                        cardsBag.Add(card);
                    }
                    else
                    {
                        // Persist the card even if details are unavailable to maintain list view integrity
                        cardsBag.Add(card);
                    }
                }
                catch (Exception)
                {
                    // Log error if necessary, but allow the process to continue for other items
                }
            });

            return cardsBag.ToList();
        }
    }
}