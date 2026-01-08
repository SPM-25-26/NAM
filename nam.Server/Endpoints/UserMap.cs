namespace nam.Server.Endpoints
{
    public static class UserMap
    {

        public static IEndpointRouteBuilder MapQuestionaire(this IEndpointRouteBuilder builder)
        {

            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            UserEndpoints.ConfigureLogger(logger);

            RouteGroupBuilder groupBuilder = builder.MapGroup("/api/user")
            .RequireCors("FrontendWithCredentials")
            .WithTags("Authentication");

            groupBuilder.MapPost("/update-questionaire", UserEndpoints.UpdateQuestionaire)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("User registration")
                .WithDescription("Registers a new user in the system with the provided details.");

            groupBuilder.MapGet("/questionaire", UserEndpoints.GetQuestionaire)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("User registration")
                .WithDescription("Registers a new user in the system with the provided details.");

            return builder;
        }
    }
}
