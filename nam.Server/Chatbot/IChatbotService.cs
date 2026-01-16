namespace nam.Server.Chatbot
{
    public interface IChatbotService
    {
        Task<string> GetResponseAsync(ChatRequest request);
    }
}
