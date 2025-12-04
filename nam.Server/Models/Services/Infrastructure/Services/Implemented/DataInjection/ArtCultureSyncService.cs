using nam.Server.Data;
using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Fetchers;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection
{
    public class ArtCultureSyncService(ApplicationDbContext dbContext, ILogger logger, IConfiguration Configuration, IHttpClientFactory httpClientFactory, string municipality)
        : BaseSyncService<ArtCultureNatureCardDto, ArtCultureNatureCard>(dbContext, logger, Configuration, municipality)
    {

        protected async override Task<List<ArtCultureNatureCard>> GetEntities(string municipality)
        {
            var query = new Dictionary<string, string?>
            {
                { "municipality", municipality }
            };
            var cardsFetcher = new ArtCultureHttpFetcher(httpClientFactory.CreateClient(), logger, Configuration, query);
            var cards = await cardsFetcher.FetchAndMapAsync();

            foreach (var card in cards)
            {
                query = new Dictionary<string, string?>
            {
                { "identifier", card.EntityId.ToString() }
            };
                var detailFetcher = new ArtCultureDetailHttpFetcher(httpClientFactory.CreateClient(), logger, Configuration, query);
                card.Detail = await detailFetcher.FetchAndMapAsync();
            }

            return cards ?? [];
        }
    }
}
