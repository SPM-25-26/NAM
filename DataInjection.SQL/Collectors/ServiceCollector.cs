using DataInjection.Core.Interfaces;
using DataInjection.Core.Providers;
using DataInjection.SQL.DTOs;
using DataInjection.SQL.Mappers;
using Domain.Entities.MunicipalityEntities;
using System.Collections.Concurrent;

namespace DataInjection.SQL.Collectors
{
    public class ServiceCollector : IEntityCollector<ServiceCard>
    {
        private readonly IFetcher _fetcher;
        private readonly IConfiguration _configuration;
        private readonly ExternalEndpointProvider<List<ServiceCardDto>, List<ServiceCard>> _cardProvider;

        public ServiceCollector(IFetcher fetcher, IConfiguration configuration)
        {
            _fetcher = fetcher;
            _configuration = configuration;

            _cardProvider = new(
                _configuration,
                fetcher,
                new ServiceCardMapper(),
                "api/services/card-list",
                new Dictionary<string, string?> { { "municipality", "" } }
            );
        }

        public async Task<List<ServiceCard>> GetEntities(string municipality)
        {
            // 1) Master list
            _cardProvider.Query["municipality"] = municipality;
            var cards = await _cardProvider.GetEntity();

            if (cards == null || !cards.Any())
                return [];

            var cardsBag = new ConcurrentBag<ServiceCard>();

            // 2) Details in parallel
            await Parallel.ForEachAsync(cards, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (card, ct) =>
            {
                var localDetailProvider = new ExternalEndpointProvider<ServiceCardDetailDto, ServiceDetail>(
                    _configuration,
                    _fetcher,
                    new ServiceCardDetailMapper(),
                    "api/services/detail/{identifier}",
                    new Dictionary<string, string?> { { "identifier", card.EntityId.ToString() } }
                );

                try
                {
                    var detail = await localDetailProvider.GetEntity();

                    if (detail != null)
                    {
                        // CRITICAL: allineamento ID per FK `ServiceCards.DetailIdentifier -> ServiceDetails.Identifier`
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