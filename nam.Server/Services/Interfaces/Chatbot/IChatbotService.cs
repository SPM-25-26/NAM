using nam.Server.Services.Implemented.Chatbot;

namespace nam.Server.Services.Interfaces.Chatbot
{
    public interface IChatbotService
    {
        Task<string> GetResponseAsync(ChatRequest request, string userEmail);
    }
}
