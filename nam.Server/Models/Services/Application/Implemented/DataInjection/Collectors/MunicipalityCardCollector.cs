using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Providers;
using nam.Server.Models.Services.Application.Interfaces.DataInjection;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Mappers;

namespace nam.Server.Models.Services.Application.Implemented.DataInjection.Collectors
{
    public class MunicipalityCardCollector(IFetcher fetcher) : IEntityCollector<MunicipalityCard>
    {
        private readonly BaseProvider<List<MunicipalityCardDto>, List<MunicipalityCard>> cardProvider = new(
           fetcher,
              new MunicipalityCardMapper(),
              "api/organizations/municipalities",
              new Dictionary<string, string?> { { "search", "" } }
          );
        private readonly BaseProvider<MunicipalityHomeInfoDto, MunicipalityHomeInfo> cardDetailProvider = new(
            fetcher,
               new MunicipalityHomeInfoMapper(),
               "api/organizations/municipalities/visit",
               new Dictionary<string, string?> { { "city", "" } }
           );

        public Task<List<MunicipalityCard>> GetEntities(string municipality)
        {
            cardProvider.Query["search"] = municipality;
            var municipalityCardList = cardProvider.GetEntity();
            foreach (var municipalityCard in municipalityCardList.Result)
            {
                cardDetailProvider.Query["city"] = municipalityCard.LegalName;
                var detail = cardDetailProvider.GetEntity();
                municipalityCard.Detail = detail.Result;
            }
            return municipalityCardList;
        }
    }
}
