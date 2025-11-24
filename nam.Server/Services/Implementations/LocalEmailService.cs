namespace nam.Server.Services.Implementations
{
    // LocalEmailService per ambienti di sviluppo/test
    public class LocalEmailService : IEmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string bodyHtml)
        {
            // Questa implementazione scrive l'output nella console/terminale
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n================== LOCAL EMAIL SENT ==================");
            Console.ResetColor();
            Console.WriteLine($"To: {toEmail}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine($"Body HTML (Simulated Send): \n{bodyHtml}");
            Console.WriteLine("======================================================\n");

            // Ritorna un Task completato immediatamente
            return Task.CompletedTask;
        }
    }
}