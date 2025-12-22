using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Providers;
using nam.Server.Models.Services.Application.Interfaces.DataInjection;
using nam.Server.Models.Services.Application.Implemented.DataInjection.Mappers;
using System.Collections.Concurrent;

namespace nam.Server.Models.Services.Application.Implemented.DataInjection.Collectors
{
    public class ArticleCollector : IEntityCollector<ArticleCard>
    {
        private readonly IFetcher _fetcher;
        private readonly BaseProvider<List<ArticleCardDto>, List<ArticleCard>> _articleProvider;

        public ArticleCollector(IFetcher fetcher)
        {
            _fetcher = fetcher;
            _articleProvider = new(
               fetcher,
               new ArticleCardMapper(),
               "api/articles/card-list",
               new Dictionary<string, string?> { { "municipality", "" } }
           );
        }

        public async Task<List<ArticleCard>> GetEntities(string municipality)
        {
            // 1. Retrieve the master list of Articles
            _articleProvider.Query["municipality"] = municipality;
            var articles = await _articleProvider.GetEntity();

            if (articles == null || !articles.Any()) return [];

            var articlesBag = new ConcurrentBag<ArticleCard>();

            // 2. Fetch Details in parallel
            await Parallel.ForEachAsync(articles, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (article, ct) =>
            {
                // Instantiate a local provider to ensure thread safety
                var localDetailProvider = new BaseProvider<ArticleDetailDto, ArticleDetail>(
                    _fetcher,
                    new ArticleDetailMapper(),
                    "api/articles/detail/{identifier}",
                    new Dictionary<string, string?> { { "identifier", article.EntityId.ToString() } }
                );

                try
                {
                    var detail = await localDetailProvider.GetEntity();

                    if (detail != null)
                    {
                        // Alignment safety: Ensure Detail ID matches Card ID
                        detail.Identifier = article.EntityId;

                        // Link the detail to the card
                        article.Detail = detail;

                        articlesBag.Add(article);
                    }
                    else
                    {
                        articlesBag.Add(article);
                    }
                }
                catch (Exception)
                {
                    // Log error if necessary
                }
            });

            return articlesBag.ToList();
        }
    }
}