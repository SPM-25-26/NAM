using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Mappers;
using nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Providers;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Collectors
{
    public class ArticleCollector(IFetcher fetcher) : IEntityCollector<ArticleCard>
    {
        private BaseProvider<List<ArticleCardDto>, List<ArticleCard>> articleProvider = new(
               fetcher,
               new ArticleCardMapper(),
               "api/articles/card-list",
               new Dictionary<string, string?> { { "municipality", "" } }
           );
        private BaseProvider<ArticleDetailDto, ArticleDetail> articleDetailProvider = new(
                fetcher,
                new ArticleDetailMapper(),
                "api/articles/detail/{identifier}",
                new Dictionary<string, string?> { { "identifier", "" } }
            );

        public Task<List<ArticleCard>> GetEntities(string municipality)
        {
            articleProvider.Query["municipality"] = municipality;
            var eventList = articleProvider.GetEntity();
            foreach (var @event in eventList.Result)
            {
                articleDetailProvider.Query["identifier"] = @event.EntityId.ToString();
                var detail = articleDetailProvider.GetEntity();
                @event.Detail = detail.Result;
            }
            return eventList;
        }
    }
}
