namespace nam.Server.Endpoints.MunicipalityEntities
{
    internal static class ShoppingMap
    {
        public static IEndpointRouteBuilder MapShopping(this IEndpointRouteBuilder builder)
        {
            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            ShoppingEndpoints.ConfigureLogger(logger);

            RouteGroupBuilder group = builder.MapGroup("/api/shopping")
                .RequireCors("FrontendWithCredentials")
                .RequireAuthorization()
                .WithTags("Shopping");

            group.MapGet("/card-list", ShoppingEndpoints.GetCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card list of shopping")
                .WithDescription("");

            group.MapGet("/detail/{identifier}", ShoppingEndpoints.GetCardDetail)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get the details of shopping card")
                .WithDescription("");

            group.MapGet("/card", ShoppingEndpoints.GetFullCard)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card of shopping")
                .WithDescription("");

            group.MapGet("/full-card-list", ShoppingEndpoints.GetFullCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get full card list of shopping")
                .WithDescription("");

            return builder;
        }
    }
}
