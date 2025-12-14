using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Providers;
using nam.Server.Models.Services.Application.Interfaces.DataInjection;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Mappers;

namespace nam.Server.Models.Services.Application.Implemented.DataInjection.Collectors
{
    public class EntertainmentLeisureCardCollector : IEntityCollector<EntertainmentLeisureCard>
    {

        private readonly BaseProvider<List<EntertainmentLeisureCardDto>, List<EntertainmentLeisureCard>> cardProvider;
        private readonly BaseProvider<EntertainmentLeisureDetailDto, EntertainmentLeisureDetail> cardDetailProvider;

        public EntertainmentLeisureCardCollector(IFetcher fetcher)
        {
            cardProvider = new(
               fetcher,
               new EntertainmentLeisureCardMapper(),
               "api/entertainment-leisure/card-list",
               new Dictionary<string, string?> { { "municipality", "" } }
           );

            cardDetailProvider = new(
                fetcher,
                new EntertainmentLeisureDetailMapper(),
                "api/entertainment-leisure/detail/{identifier}",
                new Dictionary<string, string?> { { "identifier", "" } }
            );
        }

        public Task<List<EntertainmentLeisureCard>> GetEntities(string municipality)
        {
            cardProvider.Query["municipality"] = municipality;
            var entertainmentList = cardProvider.GetEntity();
            foreach (var entertainment in entertainmentList.Result)
            {
                cardDetailProvider.Query["identifier"] = entertainment.EntityId.ToString();
                var detail = cardDetailProvider.GetEntity();
                entertainment.Detail = detail.Result;
            }
            return entertainmentList;
        }
    }
}
