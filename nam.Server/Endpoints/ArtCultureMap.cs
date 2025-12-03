namespace nam.Server.Endpoints
{
    internal static class ArtCultureMap
    {
        public static IEndpointRouteBuilder MapArtCulture(this IEndpointRouteBuilder builder)
        {

            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            ArtCultureEndpoints.ConfigureLogger(logger);

            // Quando mappi il gruppo di endpoint
            RouteGroupBuilder group = builder.MapGroup("/api/art-culture")
                .RequireCors("FrontendWithCredentials")
                //.RequireAuthorization() 
                .WithTags("Art and Culture");

            group.MapGet("/card-list", ArtCultureEndpoints.GetCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card list of art culture")
                .WithDescription("");

            group.MapGet("/detail/{identifier}", ArtCultureEndpoints.GetCardDetail)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get the details of art culture card")
                .WithDescription("");

            return builder;
        }
    }
}
