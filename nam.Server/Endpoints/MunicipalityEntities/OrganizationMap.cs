namespace nam.Server.Endpoints.MunicipalityEntities
{
    internal static class OrganizationMap
    {
        public static IEndpointRouteBuilder MapOrganization(this IEndpointRouteBuilder builder)
        {
            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            OrganizationEndpoints.ConfigureLogger(logger);

            RouteGroupBuilder group = builder.MapGroup("/api/organizations")
                .RequireCors("FrontendWithCredentials")
                .RequireAuthorization()
                .WithTags("Organization");

            group.MapGet("/card-list", OrganizationEndpoints.GetCardList)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card list of organization")
                .WithDescription("");

            group.MapGet("/detail/{taxCode}", OrganizationEndpoints.GetCardDetail)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get the details of organization card")
                .WithDescription("");

            group.MapGet("/card", OrganizationEndpoints.GetFullCard)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get card of organization")
                .WithDescription("");

            return builder;
        }
    }
}
