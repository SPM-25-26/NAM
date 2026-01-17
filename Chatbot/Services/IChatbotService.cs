namespace Chatbot.Services
{
    public interface IChatbotService
    {
        Task<string> GetResponseAsync(ChatRequest request, string userEmail);
    }
}
