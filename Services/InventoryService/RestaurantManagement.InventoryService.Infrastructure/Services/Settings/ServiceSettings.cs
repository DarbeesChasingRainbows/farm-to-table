namespace RestaurantManagement.InventoryService.Infrastructure.Services.Settings
{
    /// <summary>
    /// Settings for the email service.
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
    /// Settings for the file storage service.
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
