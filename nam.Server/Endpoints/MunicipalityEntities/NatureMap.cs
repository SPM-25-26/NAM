namespace nam.Server.Endpoints.MunicipalityEntities
{
    internal static class NatureMap
    {
        public static IEndpointRouteBuilder MapNature(this IEndpointRouteBuilder builder)
        {
            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            NatureEndpoints.ConfigureLogger(logger);

            RouteGroupBuilder group = builder.MapGroup("/api/nature")
                .RequireCors("FrontendWithCredentials")
                .RequireAuthorization()
                .WithTags("Nature");

            group.MapGet("/card-list", NatureEndpoints.GetCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card list of nature")
                .WithDescription("");

            group.MapGet("/detail/{identifier}", NatureEndpoints.GetCardDetail)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get the details of nature card")
                .WithDescription("");

            group.MapGet("/card", NatureEndpoints.GetFullCard)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card of nature")
                .WithDescription("");

            return builder;
        }
    }
}
