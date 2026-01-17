namespace nam.Server.Endpoints.Chatbot
{
    internal static class ChatbotMap
    {
        public static IEndpointRouteBuilder MapChatbot(this IEndpointRouteBuilder builder)
        {
            var logger = builder.ServiceProvider.GetService<Serilog.ILogger>() ?? Serilog.Log.Logger;
            ChatbotEndpoints.ConfigureLogger(logger);

            RouteGroupBuilder groupBuilder = builder.MapGroup("/api/assistant")
            .RequireCors("FrontendWithCredentials")
            .WithTags("Chatbot");

            groupBuilder.MapPost("/chat", ChatbotEndpoints.GetResponse)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Chat with chatbot")
            .WithDescription("Chat with the chatbot");

            return builder;
        }
    }
}
