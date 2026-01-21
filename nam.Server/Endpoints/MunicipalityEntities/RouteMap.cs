namespace nam.Server.Endpoints.MunicipalityEntities
{
    internal static class RouteMap
    {
        public static IEndpointRouteBuilder MapRoute(this IEndpointRouteBuilder builder)
        {
            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            RouteEndpoints.ConfigureLogger(logger);

            RouteGroupBuilder group = builder.MapGroup("/api/routes")
                .RequireCors("FrontendWithCredentials")
                .RequireAuthorization()
                .WithTags("Routes");

            group.MapGet("/card-list", RouteEndpoints.GetCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card list of routes")
                .WithDescription("");

            group.MapGet("/detail/{identifier}", RouteEndpoints.GetCardDetail)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get the details of route card")
                .WithDescription("");

            group.MapGet("/card", RouteEndpoints.GetFullCard)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card of routes")
                .WithDescription("");

            group.MapGet("/full-card-list", RouteEndpoints.GetFullCardList)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get full card list of routes")
            .WithDescription("");

            return builder;
        }
    }
}
