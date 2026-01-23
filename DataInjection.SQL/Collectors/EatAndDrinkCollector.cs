using DataInjection.Core.Interfaces;
using DataInjection.Core.Providers;
using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;
using System.Collections.Concurrent;

namespace DataInjection.SQL.Collectors
{
    public class EatAndDrinkCollector : IEntityCollector<EatAndDrinkCard>
    {
        private readonly IFetcher _fetcher;
        private readonly IConfiguration _configuration;
        private readonly ExternalEndpointProvider<List<EatAndDrinkCardDto>, List<EatAndDrinkCard>> _cardProvider;

        public EatAndDrinkCollector(IFetcher fetcher, IConfiguration configuration)
        {
            _fetcher = fetcher;
            _configuration = configuration;

            _cardProvider = new(
                _configuration,
                fetcher,
                new EatAndDrinkCardMapper(),
                "api/eat-and-drink/card-list",
                new Dictionary<string, string?> { { "municipality", "" } }
            );
        }

        public async Task<List<EatAndDrinkCard>> GetEntities(string municipality)
        {
            // 1) Master list
            _cardProvider.Query["municipality"] = municipality;
            var cards = await _cardProvider.GetEntity();

            if (cards == null || !cards.Any())
                return [];

            var cardsBag = new ConcurrentBag<EatAndDrinkCard>();

            // 2) Details in parallel
            await Parallel.ForEachAsync(cards, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (card, ct) =>
            {
                var localDetailProvider = new ExternalEndpointProvider<EatAndDrinkDetailDto, EatAndDrinkDetail>(
                    _configuration,
                    _fetcher,
                    new EatAndDrinkDetailMapper(),
                    "api/eat-and-drink/detail/{identifier}",
                    new Dictionary<string, string?> { { "identifier", card.EntityId.ToString() } }
                );

                try
                {
                    var detail = await localDetailProvider.GetEntity();

                    if (detail != null)
                    {
                        // CRITICAL: allineamento ID per FK `EatAndDrinkCards.DetailIdentifier -> EatAndDrinkDetails.Identifier`
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
