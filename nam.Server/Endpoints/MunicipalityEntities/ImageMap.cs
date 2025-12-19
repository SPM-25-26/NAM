namespace nam.Server.Endpoints.MunicipalityEntities
{
    internal static class ImageMap
    {
        public static IEndpointRouteBuilder MapImages(this IEndpointRouteBuilder builder)
        {
            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            ImageEndpoints.ConfigureLogger(logger);

            RouteGroupBuilder group = builder.MapGroup("/api/image")
                .RequireCors("FrontendWithCredentials")
                .RequireAuthorization() 
                .WithTags("Image");

            group.MapGet("/external", ImageEndpoints.GetExternalImage)
                .WithName("GetExternalImage")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);

            return builder;
        }
    }
}
