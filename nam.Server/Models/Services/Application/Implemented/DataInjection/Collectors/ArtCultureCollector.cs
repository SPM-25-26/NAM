using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Providers;
using nam.Server.Models.Services.Application.Interfaces.DataInjection;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Mappers;

namespace nam.Server.Models.Services.Application.Implemented.DataInjection.Collectors
{
    public class ArtCultureCollector : IEntityCollector<ArtCultureNatureCard>
    {

        private readonly BaseProvider<List<ArtCultureNatureCardDto>, List<ArtCultureNatureCard>> cardProvider;
        private readonly BaseProvider<ArtCultureNatureDetailDto, ArtCultureNatureDetail> cardDetailProvider;

        public ArtCultureCollector(IFetcher fetcher)
        {
            cardProvider = new(
               fetcher,
               new ArtCultureCardMapper(),
               "api/art-culture/card-list",
               new Dictionary<string, string?> { { "municipality", "" } }
           );

            cardDetailProvider = new(
                fetcher,
                new ArtCultureCardDetailMapper(),
                "api/art-culture/detail/{identifier}",
                new Dictionary<string, string?> { { "identifier", "" } }
            );
        }

        public Task<List<ArtCultureNatureCard>> GetEntities(string municipality)
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
