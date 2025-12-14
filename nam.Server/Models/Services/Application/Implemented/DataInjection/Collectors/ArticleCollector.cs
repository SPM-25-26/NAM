using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Providers;
using nam.Server.Models.Services.Application.Interfaces.DataInjection;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Mappers;

namespace nam.Server.Models.Services.Application.Implemented.DataInjection.Collectors
{
    public class ArticleCollector(IFetcher fetcher) : IEntityCollector<ArticleCard>
    {
        private readonly BaseProvider<List<ArticleCardDto>, List<ArticleCard>> articleProvider = new(
               fetcher,
               new ArticleCardMapper(),
               "api/articles/card-list",
               new Dictionary<string, string?> { { "municipality", "" } }
           );
        private readonly BaseProvider<ArticleDetailDto, ArticleDetail> articleDetailProvider = new(
                fetcher,
                new ArticleDetailMapper(),
                "api/articles/detail/{identifier}",
                new Dictionary<string, string?> { { "identifier", "" } }
            );

        public Task<List<ArticleCard>> GetEntities(string municipality)
        {
            articleProvider.Query["municipality"] = municipality;
            var articleList = articleProvider.GetEntity();
            foreach (var article in articleList.Result)
            {
                articleDetailProvider.Query["identifier"] = article.EntityId.ToString();
                var detail = articleDetailProvider.GetEntity();
                article.Detail = detail.Result;
            }
            return articleList;
        }
    }
}
