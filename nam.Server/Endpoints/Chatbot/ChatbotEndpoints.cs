using Microsoft.AspNetCore.Mvc;
using nam.Server.Chatbot;

namespace nam.Server.Endpoints.Chatbot
{
    internal static class ChatbotEndpoints
    {
        private static Serilog.ILogger? _logger;

        public static void ConfigureLogger(Serilog.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public static async Task<IResult> GetResponse(
            [FromBody] ChatRequest request,
            IChatbotService chatbotService)
        {
            ArgumentNullException.ThrowIfNull(chatbotService);
            try
            {
                if (request == null)
                {
                    _logger?.Warning("RegisterUser called chatbot with null request");
                    return TypedResults.Problem(detail: "Request body cannot be null.", statusCode: 400);
                }
                var response = await chatbotService.GetResponseAsync(request);
                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error while contacting chatbot");
                return TypedResults.Problem(detail: "An error occurred while contacting chatbot.", statusCode: 500);
            }
        }
    }
}
