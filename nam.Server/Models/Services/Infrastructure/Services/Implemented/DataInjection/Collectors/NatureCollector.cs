using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Mappers;
using nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Providers;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Collectors
{
    public class NatureCollector : IEntityCollector<Nature>
    {
        private BaseProvider<List<ArtCultureNatureCardDto>, List<Nature>> cardProvider;
        private BaseProvider<ArtCultureNatureDetailDto, ArtCultureNatureDetail> cardDetailProvider;

        public NatureCollector(IFetcher fetcher)
        {
            cardProvider = new(
               fetcher,
               new NatureMapper(),
               "api/nature/card-list",
               new Dictionary<string, string?> { { "municipality", "" } }
           );

            cardDetailProvider = new(
                fetcher,
                new ArtCultureCardDetailMapper(),
                "api/nature/detail/{identifier}",
                new Dictionary<string, string?> { { "identifier", "" } }
            );
        }

        public Task<List<Nature>> GetEntities(string municipality)
        {
            cardProvider.Query["municipality"] = municipality;
            var artCultureList = cardProvider.GetEntity();
            foreach (var artCulture in artCultureList.Result)
            {
                cardDetailProvider.Query["identifier"] = artCulture.EntityId.ToString();
                var detail = cardDetailProvider.GetEntity();
                artCulture.Detail = detail.Result;
            }
            return artCultureList;
        }
    }
}