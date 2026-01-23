namespace nam.Server.Endpoints
{
    public static class RecMap
    {
        public static IEndpointRouteBuilder ReccomandationMap(this IEndpointRouteBuilder builder)
        {

            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            RecEndpoints.ConfigureLogger(logger);

            RouteGroupBuilder groupBuilder = builder.MapGroup("/api/user")
            .RequireCors("FrontendWithCredentials")
            .RequireAuthorization()
            .WithTags("Recommendation");

            groupBuilder.MapGet("/get-rec", RecEndpoints.GetRec)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError);

            return builder;
        }
    }
}
