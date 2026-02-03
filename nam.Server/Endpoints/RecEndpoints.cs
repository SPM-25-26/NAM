using Microsoft.AspNetCore.Mvc;
using nam.Server.Services.Interfaces.RecSys;
using System.Security.Claims;

namespace nam.Server.Endpoints
{
    public class RecEndpoints
    {
        private static Serilog.ILogger? _logger;

        public static void ConfigureLogger(Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public static async Task<IResult> GetRec(
            [FromServices] IRecSysService recsysService,
            HttpContext httpContext,
            [FromQuery] double? lat,
            [FromQuery] double? lon,
            CancellationToken cancellationToken = default)
        {
            if (_logger is null)
            {
                throw new InvalidOperationException("Logger not configured. Please call ConfigureLogger before using this endpoint.");
            }
            var userEmail = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.Warning("User email claim is missing.");
                return TypedResults.Unauthorized();
            }
            if (lat.HasValue && lon.HasValue)
            {
                var resultWithLocation = await recsysService.GetRecommendationsAsync(userEmail, lat, lon);
                return TypedResults.Ok(resultWithLocation);
            }
            var result = await recsysService.GetRecommendationsAsync(userEmail, null, null);
            return TypedResults.Ok(result);
        }
    }
}
