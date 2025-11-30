namespace nam.Server.Endpoints
{
    internal static class PoiMap
    {
        public static IEndpointRouteBuilder MapPoi(this IEndpointRouteBuilder builder)
        {

            RouteGroupBuilder groupBuilder = builder.MapGroup("/api/poi").RequireCors("FrontendWithCredentials");

            groupBuilder.MapGet("/poiList", () => Results.Ok(new {message = "ok"}))//mock for now
                .RequireAuthorization()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithOpenApi(op =>
                {
                    op.Summary = "Poi list.";
                    return op;
                });

            return builder;
        }



    }
}
