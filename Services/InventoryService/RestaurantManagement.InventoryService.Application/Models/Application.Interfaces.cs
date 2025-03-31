using RestaurantManagement.InventoryService.Application.Models;

namespace RestaurantManagement.InventoryService.Application.Interfaces
{
    /// <summary>
    /// Interface for the date time service.
    /// </summary>
    public interface IDateTimeService
    {
        /// <summary>
        /// Gets the current UTC date and time.
        /// </summary>
        DateTime Now { get; }
    }

    /// <summary>
    /// Interface for the current user service.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the current user ID.
        /// </summary>
        string? UserId { get; }

        /// <summary>
        /// Gets a value indicating whether the current user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Gets the user roles.
        /// </summary>
        IEnumerable<string> Roles { get; }

        /// <summary>
        /// Checks if the current user has a specific role.
        /// </summary>
        /// <param name="role">The role to check.</param>
        /// <returns>True if the user has the role, otherwise false.</returns>
        bool IsInRole(string role);
    }

    /// <summary>
    /// Interface for the email service.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="emailMessage">The email message to send.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendEmailAsync(EmailMessage emailMessage);
    }

    /// <summary>
    /// Interface for the notification service.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Sends a notification.
        /// </summary>
        /// <param name="notification">The notification to send.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendNotificationAsync(Notification notification);
    }

    /// <summary>
    /// Interface for the file storage service.
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// Uploads a file to storage.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="content">The file content.</param>
        /// <returns>A task representing the asynchronous operation, with the file URL as the result.</returns>
        Task<string> UploadFileAsync(string fileName, string contentType, Stream content);

        /// <summary>
        /// Downloads a file from storage.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        /// <returns>A task representing the asynchronous operation, with the file content as the result.</returns>
        Task<(Stream Content, string ContentType, string FileName)> DownloadFileAsync(
            string fileUrl
        );

        /// <summary>
        /// Deletes a file from storage.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteFileAsync(string fileUrl);
    }

    /// <summary>
    /// Interface for the barcode service.
    /// </summary>
    public interface IBarcodeService
    {
        /// <summary>
        /// Generates a barcode for an inventory item.
        /// </summary>
        /// <param name="sku">The SKU of the inventory item.</param>
        /// <returns>A task representing the asynchronous operation, with the barcode data as the result.</returns>
        Task<byte[]> GenerateBarcodeAsync(string sku);

        /// <summary>
        /// Reads a barcode and returns the corresponding SKU.
        /// </summary>
        /// <param name="barcodeData">The barcode data.</param>
        /// <returns>A task representing the asynchronous operation, with the SKU as the result.</returns>
        Task<string> ReadBarcodeAsync(byte[] barcodeData);

        /// <summary>
        /// Validates whether a barcode is valid.
        /// </summary>
        /// <param name="barcodeData">The barcode data.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating whether the barcode is valid.</returns>
        Task<bool> ValidateBarcodeAsync(byte[] barcodeData);
    }
}
