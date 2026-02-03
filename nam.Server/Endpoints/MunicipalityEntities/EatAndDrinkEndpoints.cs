using Domain.Entities.MunicipalityEntities;
using Microsoft.AspNetCore.Mvc;
using nam.Server.Services.Interfaces.MunicipalityEntities;

namespace nam.Server.Endpoints.MunicipalityEntities
{
    public class EatAndDrinkEndpoints
    {
        private static Serilog.ILogger? _logger;

        public static void ConfigureLogger(Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public static async Task<IResult> GetCardList(
            [FromServices] IMunicipalityEntityService<EatAndDrinkCard, EatAndDrinkDetail> eatAndDrinkService,
            [FromQuery] string municipality,
            [FromQuery] string language = "it"
            )
        {
            try
            {
                var result = await eatAndDrinkService.GetCardListAsync(municipality, language);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error in getCardList municipality={Municipality}, language={Language}", municipality, language);
                return TypedResults.Problem(detail: "Internal server error", statusCode: 500);
            }
        }

        public static async Task<IResult> GetCardDetail(
            [FromServices] IMunicipalityEntityService<EatAndDrinkCard, EatAndDrinkDetail> eatAndDrinkService,
            [FromRoute] string identifier,
            [FromQuery] string language = "it"
            )
        {
            try
            {
                var result = await eatAndDrinkService.GetCardDetailAsync(identifier, language);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error in getCardDetail identifier={identifier}, language={Language}", identifier, language);
                return TypedResults.Problem(detail: "Internal server error", statusCode: 500);
            }
        }

        public static async Task<IResult> GetFullCard(
            [FromServices] IMunicipalityEntityService<EatAndDrinkCard, EatAndDrinkDetail> eatAndDrinkService,
            [FromQuery] string identifier,
            [FromQuery] string language = "it"
            )
        {
            try
            {
                var result = await eatAndDrinkService.GetFullCardAsync(identifier, language);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error in GetFullCard identifier={Identifier}, language={Language}", identifier, language);
                return TypedResults.Problem(detail: "Internal server error", statusCode: 500);
            }
        }

        public static async Task<IResult> GetFullCardList(
            [FromServices] IMunicipalityEntityService<EatAndDrinkCard, EatAndDrinkDetail> eatAndDrinkService,
            [FromQuery] string municipality,
            [FromQuery] string language = "it"
            )
        {
            try
            {
                var result = await eatAndDrinkService.GetFullCardListAsync(municipality, language);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error in GetFullCardList municipality={Municipality}, language={Language}", municipality, language);
                return TypedResults.Problem(detail: "Internal server error", statusCode: 500);
            }
        }
    }
}
