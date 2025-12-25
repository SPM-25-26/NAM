using DataInjection.DTOs;
using DataInjection.Interfaces;
using DataInjection.Mappers;
using DataInjection.Providers;
using Domain.Entities.MunicipalityEntities;
using System.Collections.Concurrent;

namespace DataInjection.Collectors
{
    public class OrganizationCollector : IEntityCollector<OrganizationCard>
    {
        private readonly IFetcher _fetcher;
        private readonly BaseProvider<List<OrganizationCardDto>, List<OrganizationCard>> _cardProvider;

        public OrganizationCollector(IFetcher fetcher)
        {
            _fetcher = fetcher;
            _cardProvider = new(
               fetcher,
               new OrganizationCardMapper(),
               "api/organizations/card-list",
               new Dictionary<string, string?> { { "municipality", "" } }
           );
        }

        public async Task<List<OrganizationCard>> GetEntities(string municipality)
        {
            // 1. Retrieve the master list of Organization cards
            _cardProvider.Query["municipality"] = municipality;
            var organizationCardList = await _cardProvider.GetEntity();

            if (organizationCardList == null || !organizationCardList.Any()) return [];

            var cardsBag = new ConcurrentBag<OrganizationCard>();

            // 2. Fetch Details in parallel
            await Parallel.ForEachAsync(organizationCardList, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (organizationCard, ct) =>
            {
                // Instantiate a local provider to ensure thread safety (avoid race conditions on Query dictionary)
                var localDetailProvider = new BaseProvider<OrganizationMobileDetailDto, OrganizationMobileDetail>(
                    _fetcher,
                    new OrganizationMobileDetailMapper(),
                    "api/organizations/detail/{taxcode}",
                    new Dictionary<string, string?> { { "taxcode", organizationCard.TaxCode } }
                );

                try
                {
                    var detail = await localDetailProvider.GetEntity();

                    if (detail != null)
                    {
                        // CRITICAL: Align the Detail Key with the Parent Key.
                        // Assuming TaxCode is the Primary Key or the joining key for Organizations.
                        // This ensures the SyncService recognizes the relationship correctly.
                        detail.TaxCode = organizationCard.TaxCode;

                        // Link the fetched detail to the parent card
                        organizationCard.Detail = detail;

                        cardsBag.Add(organizationCard);
                    }
                    else
                    {
                        // Persist the card even if detail fetch fails
                        cardsBag.Add(organizationCard);
                    }
                }
                catch (Exception)
                {
                    // Log error if necessary
                }
            });

            return cardsBag.ToList();
        }
    }
}