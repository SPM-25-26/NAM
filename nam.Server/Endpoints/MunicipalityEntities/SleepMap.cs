namespace nam.Server.Endpoints.MunicipalityEntities
{
    internal static class SleepMap
    {
        public static IEndpointRouteBuilder MapSleep(this IEndpointRouteBuilder builder)
        {
            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            SleepEndpoints.ConfigureLogger(logger);

            RouteGroupBuilder group = builder.MapGroup("/api/sleep")
                .RequireCors("FrontendWithCredentials")
                .RequireAuthorization()
                .WithTags("Sleep");

            group.MapGet("/card-list", SleepEndpoints.GetCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card list of sleep")
                .WithDescription("");

            group.MapGet("/detail/{identifier}", SleepEndpoints.GetCardDetail)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get the details of sleep card")
                .WithDescription("");

            group.MapGet("/card", SleepEndpoints.GetFullCard)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card of sleep")
                .WithDescription("");

            group.MapGet("/full-card-list", SleepEndpoints.GetFullCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get full card list of sleep")
                .WithDescription("");

            return builder;
        }
    }
}
