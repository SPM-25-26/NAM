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
            .RequireAuthorization()
            .WithTags("Authentication");

            groupBuilder.MapPost("/update-questionaire", UserEndpoints.UpdateQuestionaire)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Aggiorna il questionario utente")
                .WithDescription("Aggiorna le risposte del questionario per l'utente autenticato.");

            groupBuilder.MapGet("/questionaire", UserEndpoints.GetQuestionaire)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Recupera il questionario utente")
                .WithDescription("Restituisce i dati del questionario per l'utente autenticato.");
            groupBuilder.MapGet("/questionaire-completed", UserEndpoints.QuestionaireCompleted)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Recupera il questionario utente e verifica se è completo");

            return builder;
        }
    }
}
