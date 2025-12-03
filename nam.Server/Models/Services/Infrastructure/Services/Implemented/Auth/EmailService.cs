using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.Auth;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.Auth
{

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // Create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:From"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            // Send email via SMTP
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            // Connection
            await smtp.ConnectAsync(
                _config["EmailSettings:Host"],
                int.Parse(_config["EmailSettings:Port"]),
                SecureSocketOptions.StartTls
            );

            // Autentication
            await smtp.AuthenticateAsync(
                _config["EmailSettings:Username"],
                _config["EmailSettings:Password"]
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
