using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Application.Models
{
    /// <summary>
    /// Represents an email message.
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// Gets or sets the email subject.
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email body.
        /// </summary>
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the recipient email addresses.
        /// </summary>
        public List<string> ToAddresses { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the CC email addresses.
        /// </summary>
        public List<string>? CcAddresses { get; set; }

        /// <summary>
        /// Gets or sets the BCC email addresses.
        /// </summary>
        public List<string>? BccAddresses { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the email body is HTML.
        /// </summary>
        public bool IsHtml { get; set; } = false;

        /// <summary>
        /// Gets or sets the file paths for the email attachments.
        /// </summary>
        public List<string>? Attachments { get; set; }
    }

    /// <summary>
    /// Represents a notification.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Gets or sets the notification type.
        /// </summary>
        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the notification subject.
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the notification message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the recipient identifiers (email addresses, device tokens, phone numbers, etc.).
        /// </summary>
        public List<string> Recipients { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether the notification content is HTML.
        /// </summary>
        public bool IsHtml { get; set; } = false;

        /// <summary>
        /// Gets or sets the notification data.
        /// </summary>
        public Dictionary<string, string>? Data { get; set; }
    }

    /// <summary>
    /// Represents the type of notification.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Email notification.
        /// </summary>
        Email,

        /// <summary>
        /// Push notification.
        /// </summary>
        Push,

        /// <summary>
        /// SMS notification.
        /// </summary>
        Sms
    }

    /// <summary>
    /// Represents the settings for email service.
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// Gets or sets the SMTP server.
        /// </summary>
        public string SmtpServer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable SSL.
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the from address.
        /// </summary>
        public string FromAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the from name.
        /// </summary>
        public string FromName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents the settings for file storage service.
    /// </summary>
    public class FileStorageSettings
    {
        /// <summary>
        /// Gets or sets the base path.
        /// </summary>
        public string BasePath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the inventory documents path.
        /// </summary>
        public string InventoryDocumentsPath { get; set; } = "inventory-documents";

        /// <summary>
        /// Gets or sets the maximum file size in bytes.
        /// </summary>
        public long MaxFileSize { get; set; } = 10 * 1024 * 1024; // 10 MB

        /// <summary>
        /// Gets or sets the allowed file extensions.
        /// </summary>
        public string[] AllowedExtensions { get; set; } =
            {
                ".pdf",
                ".jpg",
                ".jpeg",
                ".png",
                ".gif",
                ".doc",
                ".docx",
                ".xls",
                ".xlsx",
                ".csv",
                ".txt"
            };
    }
}
