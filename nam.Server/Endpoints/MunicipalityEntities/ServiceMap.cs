namespace nam.Server.Endpoints.MunicipalityEntities
{
    internal static class ServiceMap
    {
        public static IEndpointRouteBuilder MapService(this IEndpointRouteBuilder builder)
        {
            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            ServiceEndpoints.ConfigureLogger(logger);

            RouteGroupBuilder group = builder.MapGroup("/api/services")
                .RequireCors("FrontendWithCredentials")
                .RequireAuthorization()
                .WithTags("Services");

            group.MapGet("/card-list", ServiceEndpoints.GetCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card list of services")
                .WithDescription("");

            group.MapGet("/detail/{identifier}", ServiceEndpoints.GetCardDetail)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get the details of service card")
                .WithDescription("");

            group.MapGet("/card", ServiceEndpoints.GetFullCard)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card of services")
                .WithDescription("");

            group.MapGet("/full-card-list", ServiceEndpoints.GetFullCardList)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Get full card list of services")
            .WithDescription("");

            return builder;
        }
    }
}
