using Domain.Entities.MunicipalityEntities;
using Microsoft.AspNetCore.Mvc;
using nam.Server.Services.Interfaces.MunicipalityEntities;

namespace nam.Server.Endpoints.MunicipalityEntities
{
    public class MapDataEndpoints
    {
        private static Serilog.ILogger? _logger;

        public static void ConfigureLogger(Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public static async Task<IResult> GetCardList(
            [FromServices] IMapDataService mapService,
            [FromQuery] string municipality,
            [FromQuery] string language = "it"
            )
        {
            try
            {
                var result = await mapService.GetCardAsync(municipality, language);
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error in getCardList municipality={Municipality}, language={Language}", municipality, language);
                return TypedResults.Problem(detail: "Internal server error", statusCode: 500);
            }
        }

    }
}
