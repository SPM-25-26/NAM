namespace nam.Server.Endpoints.MunicipalityEntities
{
    internal static class EatAndDrinkMap
    {
        public static IEndpointRouteBuilder MapEatAndDrink(this IEndpointRouteBuilder builder)
        {

            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            EatAndDrinkEndpoints.ConfigureLogger(logger);

            // When mapping the endpoint group
            RouteGroupBuilder group = builder.MapGroup("/api/eat-and-drink")
                .RequireCors("FrontendWithCredentials")
                .RequireAuthorization()
                .WithTags("Eat and Drink");

            group.MapGet("/card-list", EatAndDrinkEndpoints.GetCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card list of eat and drink")
                .WithDescription("");

            group.MapGet("/detail/{identifier}", EatAndDrinkEndpoints.GetCardDetail)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get the details of eat and drink card")
                .WithDescription("");

            group.MapGet("/card", EatAndDrinkEndpoints.GetFullCard)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card of eat and drink")
                .WithDescription("");

            group.MapGet("/full-card-list", EatAndDrinkEndpoints.GetFullCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get full card list of eat and drink")
                .WithDescription("");

            return builder;
        }
    }
}
