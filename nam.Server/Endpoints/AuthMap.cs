namespace nam.Server.Endpoints
{
    internal static class AuthMap
    {
        public static IEndpointRouteBuilder MapAuth(this IEndpointRouteBuilder builder)
        {
            RouteGroupBuilder groupBuilder = builder.MapGroup("/api/auth");

            groupBuilder.MapPost("/register", AuthEndpoints.RegisterUser)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithOpenApi(op =>
                {
                    op.Summary = "User registration.";
                    return op;
                });

            // POST /api/auth/login
            groupBuilder.MapPost("/login", AuthEndpoints.GenerateToken)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithOpenApi(op =>
                {
                    op.Summary = "JWT generation.";
                    return op;
                });

            return builder;
        }

    }
}
