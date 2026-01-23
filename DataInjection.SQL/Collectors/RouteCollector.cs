using DataInjection.Core.Interfaces;
using DataInjection.Core.Providers;
using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;
using System.Collections.Concurrent;

namespace DataInjection.SQL.Collectors
{
    public class RouteCollector : IEntityCollector<RouteCard>
    {
        private readonly IFetcher _fetcher;
        private readonly IConfiguration _configuration;
        private readonly ExternalEndpointProvider<List<RoutesCardDto>, List<RouteCard>> _cardProvider;

        public RouteCollector(IFetcher fetcher, IConfiguration configuration)
        {
            _fetcher = fetcher;
            _configuration = configuration;

            _cardProvider = new(
                _configuration,
                fetcher,
                new RouteCardMapper(),
                "api/routes/card-list",
                new Dictionary<string, string?> { { "municipality", "" } }
            );
        }

        public async Task<List<RouteCard>> GetEntities(string municipality)
        {
            // 1) Master list
            _cardProvider.Query["municipality"] = municipality;
            var cards = await _cardProvider.GetEntity();

            if (cards == null || !cards.Any())
                return [];

            var cardsBag = new ConcurrentBag<RouteCard>();

            // 2) Details in parallel
            await Parallel.ForEachAsync(cards, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (card, ct) =>
            {
                var localDetailProvider = new ExternalEndpointProvider<RouteDetailDto, RouteDetail>(
                    _configuration,
                    _fetcher,
                    new RouteCardDetailMapper(),
                    "api/routes/detail/{identifier}",
                    new Dictionary<string, string?> { { "identifier", card.EntityId.ToString() } }
                );

                try
                {
                    var detail = await localDetailProvider.GetEntity();

                    if (detail != null)
                    {
                        // CRITICAL: allineamento ID per FK `RouteCards.DetailIdentifier -> RouteDetails.Identifier`
                        detail.Identifier = card.EntityId;
                        card.Detail = detail;
                    }

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