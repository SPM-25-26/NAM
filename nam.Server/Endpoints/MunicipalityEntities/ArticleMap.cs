namespace nam.Server.Endpoints.MunicipalityEntities
{
    internal static class ArticleMap
    {
        public static IEndpointRouteBuilder MapArticle(this IEndpointRouteBuilder builder)
        {
            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            ArticleEndpoint.ConfigureLogger(logger);

            // When mapping the endpoint group
            RouteGroupBuilder group = builder.MapGroup("/api/article")
                .RequireCors("FrontendWithCredentials")
                .RequireAuthorization()
                .WithTags("Article");

            group.MapGet("/card-list", ArticleEndpoint.GetCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card list of articles")
                .WithDescription("");

            group.MapGet("/detail/{identifier}", ArticleEndpoint.GetCardDetail)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get the details of article card")
                .WithDescription("");

            group.MapGet("/card", ArticleEndpoint.GetFullCard)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card article")
                .WithDescription("");

            return builder;
        }
    }
}