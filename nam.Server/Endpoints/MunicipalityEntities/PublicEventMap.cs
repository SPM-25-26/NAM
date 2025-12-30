namespace nam.Server.Endpoints.MunicipalityEntities
{
    internal static class PublicEventMap
    {
        public static IEndpointRouteBuilder MapPublicEvent(this IEndpointRouteBuilder builder)
        {
            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            PublicEventEndpoint.ConfigureLogger(logger);

            RouteGroupBuilder group = builder.MapGroup("/api/public-event")
                .RequireCors("FrontendWithCredentials")
                .RequireAuthorization()
                .WithTags("Public Events");

            group.MapGet("/card-list", PublicEventEndpoint.GetCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card list of public events")
                .WithDescription("");

            group.MapGet("/detail/{identifier}", PublicEventEndpoint.GetCardDetail)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get the details of public event card")
                .WithDescription("");

            group.MapGet("/card", PublicEventEndpoint.GetFullCard)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card of public event")
                .WithDescription("");

            return builder;
        }
    }
}
