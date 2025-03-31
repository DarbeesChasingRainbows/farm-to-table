using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestaurantManagement.InventoryService.Application.Interfaces;
using RestaurantManagement.InventoryService.Application.Models;

namespace RestaurantManagement.InventoryService.Infrastructure.Services
{
    /// <summary>
    /// Implementation of the email service.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="emailSettings">The email settings.</param>
        /// <param name="logger">The logger.</param>
        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="emailMessage">The email message to send.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromAddress, _emailSettings.FromName),
                    Subject = emailMessage.Subject,
                    Body = emailMessage.Body,
                    IsBodyHtml = emailMessage.IsHtml
                };

                foreach (var to in emailMessage.ToAddresses)
                {
                    mailMessage.To.Add(new MailAddress(to));
                }

                if (emailMessage.CcAddresses != null)
                {
                    foreach (var cc in emailMessage.CcAddresses)
                    {
                        mailMessage.CC.Add(new MailAddress(cc));
                    }
                }

                if (emailMessage.BccAddresses != null)
                {
                    foreach (var bcc in emailMessage.BccAddresses)
                    {
                        mailMessage.Bcc.Add(new MailAddress(bcc));
                    }
                }

                if (emailMessage.Attachments != null)
                {
                    foreach (var attachment in emailMessage.Attachments)
                    {
                        mailMessage.Attachments.Add(new Attachment(attachment));
                    }
                }

                using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(
                        _emailSettings.Username,
                        _emailSettings.Password
                    );
                    client.EnableSsl = _emailSettings.EnableSsl;

                    await client.SendMailAsync(mailMessage);
                }

                _logger.LogInformation(
                    "Email sent successfully to {Recipients}",
                    string.Join(", ", emailMessage.ToAddresses)
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error sending email to {Recipients}",
                    string.Join(", ", emailMessage.ToAddresses)
                );
                throw;
            }
        }
    }
}
