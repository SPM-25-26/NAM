using Domain.Entities.MunicipalityEntities;
using Microsoft.AspNetCore.Mvc;
using nam.Server.Services.Interfaces.MunicipalityEntities;

namespace nam.Server.Endpoints.MunicipalityEntities
{
    public class ArticleEndpoint
    {
        private static Serilog.ILogger? _logger;

        public static void ConfigureLogger(Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public static async Task<IResult> GetCardList(
            [FromServices] IMunicipalityEntityService<ArticleCard, ArticleDetail> articleService,
            [FromQuery] string municipality,
            [FromQuery] string language = "it"
            )
        {
            try
            {
                var result = await articleService.GetCardListAsync(municipality, language);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error in getCardList municipality={Municipality}, language={Language}", municipality, language);
                return TypedResults.Problem(detail: "Internal server error", statusCode: 500);
            }
        }

        public static async Task<IResult> GetCardDetail(
            [FromServices] IMunicipalityEntityService<ArticleCard, ArticleDetail> articleService,
            [FromRoute] string identifier,
            [FromQuery] string language = "it"
            )
        {
            try
            {
                var result = await articleService.GetCardDetailAsync(identifier, language);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error in getCardDetail identifier={Identifier}, language={Language}", identifier, language);
                return TypedResults.Problem(detail: "Internal server error", statusCode: 500);
            }
        }

        public static async Task<IResult> GetFullCard(
            [FromServices] IMunicipalityEntityService<ArticleCard, ArticleDetail> articleService,
            [FromQuery] string identifier,
            [FromQuery] string language = "it"
            )
        {
            try
            {
                var result = await articleService.GetFullCardAsync(identifier, language);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error in GetFullCard identifier={Identifier}, language={Language}", identifier, language);
                return TypedResults.Problem(detail: "Internal server error", statusCode: 500);
            }
        }
    }
}
