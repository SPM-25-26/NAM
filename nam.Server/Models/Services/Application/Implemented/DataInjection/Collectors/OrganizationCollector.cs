using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Providers;
using nam.Server.Models.Services.Application.Interfaces.DataInjection;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Mappers;

namespace nam.Server.Models.Services.Application.Implemented.DataInjection.Collectors
{
    public class OrganizationCollector(IFetcher fetcher) : IEntityCollector<OrganizationCard>
    {

        private readonly BaseProvider<List<OrganizationCardDto>, List<OrganizationCard>> cardProvider = new(
           fetcher,
              new OrganizationCardMapper(),
              "api/organizations/card-list",
              new Dictionary<string, string?> { { "municipality", "" } }
          );
        private readonly BaseProvider<OrganizationMobileDetailDto, OrganizationMobileDetail> cardDetailProvider = new(
            fetcher,
               new OrganizationMobileDetailMapper(),
               "api/organizations/detail/{taxcode}",
               new Dictionary<string, string?> { { "taxcode", "" } }
           );

        public Task<List<OrganizationCard>> GetEntities(string municipality)
        {
            cardProvider.Query["municipality"] = municipality;
            var organizationCardList = cardProvider.GetEntity();
            foreach (var organizationCard in organizationCardList.Result)
            {
                cardDetailProvider.Query["taxcode"] = organizationCard.TaxCode;
                var detail = cardDetailProvider.GetEntity();
                organizationCard.Detail = detail.Result;
            }
            return organizationCardList;
        }
    }
}
