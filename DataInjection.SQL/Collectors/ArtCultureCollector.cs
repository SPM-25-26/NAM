using DataInjection.Core.Interfaces;
using DataInjection.Core.Providers;
using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;
using System.Collections.Concurrent;

namespace DataInjection.SQL.Collectors
{
    // Reverted to synchronizing the Card entity to ensure visibility in list views.
    public class ArtCultureCollector : IEntityCollector<ArtCultureNatureCard>
    {
        private readonly IFetcher _fetcher;
        private readonly IConfiguration _configuration;
        private readonly ExternalEndpointProvider<List<ArtCultureNatureCardDto>, List<ArtCultureNatureCard>> _cardProvider;

        public ArtCultureCollector(IFetcher fetcher, IConfiguration configuration)
        {
            _fetcher = fetcher;
            _configuration = configuration;
            _cardProvider = new(
                configuration,
               fetcher,
               new ArtCultureCardMapper(),
               "api/art-culture/card-list",
               new Dictionary<string, string?> { { "municipality", "" } }
            );
        }

        public async Task<List<ArtCultureNatureCard>> GetEntities(string municipality)
        {
            // 1. Retrieve the master list of Cards
            _cardProvider.Query["municipality"] = municipality;
            var cards = await _cardProvider.GetEntity();

            if (cards == null || !cards.Any()) return [];

            var cardsBag = new ConcurrentBag<ArtCultureNatureCard>();

            // 2. Fetch Details in parallel and link them to their respective Cards
            await Parallel.ForEachAsync(cards, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (card, ct) =>
            {
                // Instantiate a local provider to ensure thread safety when modifying the Query string during parallel execution.
                var localDetailProvider = new ExternalEndpointProvider<ArtCultureNatureDetailDto, ArtCultureNatureDetail>(
                    _configuration,
                    _fetcher,
                    new ArtCultureCardDetailMapper(),
                    "api/art-culture/detail/{identifier}",
                    new Dictionary<string, string?> { { "identifier", card.EntityId.ToString() } }
                );

                try
                {
                    var detail = await localDetailProvider.GetEntity();

                    if (detail != null)
                    {
                        // CRITICAL FIX: Synchronize the Detail Identifier with the Card EntityId.
                        // This ensures referential integrity and prevents ID mismatches during sync.
                        detail.Identifier = card.EntityId;

                        // LINKING: Associate the fetched Detail with the parent Card
                        card.Detail = detail;

                        // Add the populated Card (containing the Detail) to the collection for persistence
                        cardsBag.Add(card);
                    }
                    else
                    {
                        // If no detail is found, persist the Card regardless to ensure it remains available in list views.
                        cardsBag.Add(card);
                    }
                }
                catch (Exception ex)
                {
                    // Log error
                }
            });

            return cardsBag.ToList();
        }
    }
}