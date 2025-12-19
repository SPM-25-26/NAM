using Serilog;

namespace nam.Server.Endpoints.MunicipalityEntities
{
    internal static class MunicipalityCardMap
    {
        public static IEndpointRouteBuilder MapMunicipalityCard(this IEndpointRouteBuilder builder)
        {
            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Log.Logger;
            MunicipalityCardEndpoints.ConfigureLogger(logger);

            RouteGroupBuilder group = builder.MapGroup("/api/organizations/municipalities")
                .RequireCors("FrontendWithCredentials")
                .RequireAuthorization()
                .WithTags("MunicipalityCard");

            group.MapGet("", MunicipalityCardEndpoints.GetCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card list of municipality")
                .WithDescription("");

            group.MapGet("/visit", MunicipalityCardEndpoints.GetCardDetail)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get the details of municipality card")
                .WithDescription("");

            return builder;
        }
    }
}
