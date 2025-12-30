using DataInjection.DTOs;
using DataInjection.Interfaces;
using DataInjection.Mappers;
using DataInjection.Providers;
using Domain.Entities.MunicipalityEntities;
using System.Collections.Concurrent;

namespace DataInjection.Collectors
{
    public class MunicipalityCardCollector : IEntityCollector<MunicipalityCard>
    {
        private readonly IFetcher _fetcher;
        private readonly IConfiguration _configuration;
        private readonly ExternalEndpointProvider<List<MunicipalityCardDto>, List<MunicipalityCard>> _cardProvider;

        public MunicipalityCardCollector(IFetcher fetcher, IConfiguration configuration)
        {
            _fetcher = fetcher;
            _configuration = configuration;
            _cardProvider = new(
                _configuration,
               fetcher,
               new MunicipalityCardMapper(),
               "api/organizations/municipalities",
               new Dictionary<string, string?> { { "search", "" } }
           );
        }

        public async Task<List<MunicipalityCard>> GetEntities(string municipality)
        {
            // 1. Retrieve the master list of Municipalities
            _cardProvider.Query["search"] = municipality;
            var municipalityCardList = await _cardProvider.GetEntity();

            if (municipalityCardList == null || !municipalityCardList.Any()) return [];

            var cardsBag = new ConcurrentBag<MunicipalityCard>();

            // 2. Fetch HomeInfo Details in parallel
            await Parallel.ForEachAsync(municipalityCardList, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (municipalityCard, ct) =>
            {
                // Instantiate a local provider to ensure thread safety when modifying the Query string
                var localDetailProvider = new ExternalEndpointProvider<MunicipalityHomeInfoDto, MunicipalityHomeInfo>(
                    _configuration,
                    _fetcher,
                    new MunicipalityHomeInfoMapper(),
                    "api/organizations/municipalities/visit",
                    new Dictionary<string, string?> { { "city", municipalityCard.LegalName } }
                );

                try
                {
                    var detail = await localDetailProvider.GetEntity();

                    if (detail != null)
                    {
                        // CRITICAL: Force ID alignment.
                        // The Detail (HomeInfo) must share the same Identifier as the Parent Card.
                        // This ensures the SyncService correctly identifies relationships for cleanup/upsert.
                        // Assuming 'Identifier' is the PK of MunicipalityHomeInfo and 'EntityId' is the PK of MunicipalityCard.
                        detail.LegalName = municipalityCard.LegalName;

                        // Link the Detail to the Parent
                        municipalityCard.Detail = detail;

                        cardsBag.Add(municipalityCard);
                    }
                    else
                    {
                        // Persist the card even if Detail is missing
                        cardsBag.Add(municipalityCard);
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