namespace nam.Server.Endpoints.MunicipalityEntities
{
    internal static class EntertainmentLeisureMap
    {
        public static IEndpointRouteBuilder MapEntertainmentLeisure(this IEndpointRouteBuilder builder)
        {
            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            EntertainmentLeisureEndpoints.ConfigureLogger(logger);

            RouteGroupBuilder group = builder.MapGroup("/api/entertainment-leisure")
                .RequireCors("FrontendWithCredentials")
                .RequireAuthorization()
                .WithTags("Entertainment and Leisure");

            group.MapGet("/card-list", EntertainmentLeisureEndpoints.GetCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card list of entertainment and leisure")
                .WithDescription("");

            group.MapGet("/detail/{identifier}", EntertainmentLeisureEndpoints.GetCardDetail)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get the details of entertainment and leisure card")
                .WithDescription("");

            group.MapGet("/card", EntertainmentLeisureEndpoints.GetFullCard)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card of entertainment")
                .WithDescription("");

            return builder;
        }
    }
}