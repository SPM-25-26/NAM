using Microsoft.AspNetCore.Mvc;
using nam.Server.Services.Implemented.Chatbot;
using nam.Server.Services.Interfaces.Chatbot;
using System.Security.Claims;

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
            HttpContext httpContext,
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
                var userEmail = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                {
                    _logger.Warning("User email claim is missing.");
                    return TypedResults.Unauthorized();
                }

                var response = await chatbotService.GetResponseAsync(request, userEmail);
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
