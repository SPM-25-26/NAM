using Domain.Entities.MunicipalityEntities;
using Microsoft.AspNetCore.Mvc;
using nam.Server.Services.Interfaces.MunicipalityEntities;

namespace nam.Server.Endpoints.MunicipalityEntities
{
    public class MunicipalityCardEndpoints
    {
        private static Serilog.ILogger? _logger;

        public static void ConfigureLogger(Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public static async Task<IResult> GetCardList(
            [FromServices] IMunicipalityEntityService<MunicipalityCard, MunicipalityHomeInfo> municipalityService,
            [FromQuery] string search,
            [FromQuery] string language = "it"
            )
        {
            try
            {
                var result = await municipalityService.GetCardListAsync(search, language);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error in getCardList municipality={Municipality}, language={Language}", search, language);
                return TypedResults.Problem(detail: "Internal server error", statusCode: 500);
            }
        }

        public static async Task<IResult> GetCardDetail(
            [FromServices] IMunicipalityEntityService<MunicipalityCard, MunicipalityHomeInfo> municipalityService,
            [FromQuery] string city,
            [FromQuery] string language = "it"
            )
        {
            try
            {
                var result = await municipalityService.GetCardDetailAsync(city, language);
                if (result == null)
                {
                    return TypedResults.NotFound();
                }
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error in getCardDetail code={Code}, language={Language}", city, language);
                return TypedResults.Problem(detail: "Internal server error", statusCode: 500);
            }
        }
    }
}
