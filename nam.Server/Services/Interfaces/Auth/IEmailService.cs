namespace nam.Server.Services.Interfaces.Auth
{
    /// <summary>
    /// Provides functionality for sending emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email asynchronously to the specified recipient.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The subject line of the email.</param>
        /// <param name="bodyHtml">
        /// The HTML content of the email body.  
        /// This should contain valid HTML markup.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous send operation.
        /// </returns>
        Task SendEmailAsync(string toEmail, string subject, string bodyHtml);
    }
}