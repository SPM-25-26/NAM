using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Mappers;
using nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Providers;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Collectors
{
    public class PublicEventCollector : IEntityCollector<PublicEventCard>
    {
        private readonly BaseProvider<List<PublicEventCardDto>, List<PublicEventCard>> cardProvider;
        private readonly BaseProvider<PublicEventMobileDetailDto, PublicEventMobileDetail> cardDetailProvider;

        public PublicEventCollector(IFetcher fetcher)
        {
            cardProvider = new(
               fetcher,
               new PublicEventCardMapper(),
               "api/events/card-list",
               new Dictionary<string, string?> { { "municipality", "" } }
           );

            cardDetailProvider = new(
                fetcher,
                new PublicEventCardDetailMapper(),
                "api/events/detail/{identifier}",
                new Dictionary<string, string?> { { "identifier", "" } }
            );
        }

        public Task<List<PublicEventCard>> GetEntities(string municipality)
        {
            cardProvider.Query["municipality"] = municipality;
            var eventList = cardProvider.GetEntity();
            foreach (var publicEvent in eventList.Result)
            {
                cardDetailProvider.Query["identifier"] = publicEvent.EntityId.ToString();
                var detail = cardDetailProvider.GetEntity();
                publicEvent.Detail = detail.Result;
            }
            return eventList;
        }
    }
}
