using DataInjection.Core.Interfaces;
using DataInjection.Core.Providers;
using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;
using System.Collections.Concurrent;

namespace DataInjection.SQL.Collectors
{
    public class ShoppingCollector : IEntityCollector<ShoppingCard>
    {
        private readonly IFetcher _fetcher;
        private readonly IConfiguration _configuration;
        private readonly ExternalEndpointProvider<List<ShoppingCardDto>, List<ShoppingCard>> _cardProvider;

        public ShoppingCollector(IFetcher fetcher, IConfiguration configuration)
        {
            _fetcher = fetcher;
            _configuration = configuration;

            _cardProvider = new(
                _configuration,
                fetcher,
                new ShoppingCardMapper(),
                "api/shopping/card-list",
                new Dictionary<string, string?> { { "municipality", "" } }
            );
        }

        public async Task<List<ShoppingCard>> GetEntities(string municipality)
        {
            // 1) Master list
            _cardProvider.Query["municipality"] = municipality;
            var cards = await _cardProvider.GetEntity();

            if (cards == null || !cards.Any())
                return [];

            var cardsBag = new ConcurrentBag<ShoppingCard>();

            // 2) Details in parallel
            await Parallel.ForEachAsync(cards, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (card, ct) =>
            {
                var localDetailProvider = new ExternalEndpointProvider<ShoppingCardDetailDto, ShoppingCardDetail>(
                    _configuration,
                    _fetcher,
                    new ShoppingCardDetailMapper(),
                    "api/shopping/detail/{identifier}",
                    new Dictionary<string, string?> { { "identifier", card.EntityId.ToString() } }
                );

                try
                {
                    var detail = await localDetailProvider.GetEntity();

                    if (detail != null)
                    {
                        // CRITICAL: allineamento ID per FK `ShoppingCards.DetailIdentifier -> ShoppingCardDetails.Identifier`
                        detail.Identifier = card.EntityId;
                        card.Detail = detail;
                    }

                    // Persisti sempre la card (anche se detail è null) per list-view
                    cardsBag.Add(card);
                }
                catch (Exception)
                {
                    // Mantieni comunque la card per la list-view
                    cardsBag.Add(card);
                }
            });

            return cardsBag.ToList();
        }
    }
}
