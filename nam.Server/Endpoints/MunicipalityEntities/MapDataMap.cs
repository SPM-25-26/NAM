namespace nam.Server.Endpoints.MunicipalityEntities
{
    internal static class MapDataMap
    {
        public static IEndpointRouteBuilder MapMapData(this IEndpointRouteBuilder builder)
        {

            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            MapDataEndpoints.ConfigureLogger(logger);

            // When mapping the endpoint group
            RouteGroupBuilder group = builder.MapGroup("/api/map")
                .RequireCors("FrontendWithCredentials")
                .RequireAuthorization()
                .WithTags("Map");

            group.MapGet("/", MapDataEndpoints.GetCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get map data")
                .WithDescription("");

            return builder;
        }
    }
}
