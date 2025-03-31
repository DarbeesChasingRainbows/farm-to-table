using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestaurantManagement.InventoryService.Application.Interfaces;
using RestaurantManagement.InventoryService.Application.Models;

namespace RestaurantManagement.InventoryService.Infrastructure.Services
{
    /// <summary>
    /// Implementation of the file storage service.
    /// </summary>
    public class FileStorageService : IFileStorageService
    {
        private readonly FileStorageSettings _fileStorageSettings;
        private readonly ILogger<FileStorageService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStorageService"/> class.
        /// </summary>
        /// <param name="fileStorageSettings">The file storage settings.</param>
        /// <param name="logger">The logger.</param>
        public FileStorageService(
            IOptions<FileStorageSettings> fileStorageSettings,
            ILogger<FileStorageService> logger
        )
        {
            _fileStorageSettings = fileStorageSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Uploads a file to storage.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="content">The file content.</param>
        /// <returns>A task representing the asynchronous operation, with the file URL as the result.</returns>
        public async Task<string> UploadFileAsync(
            string fileName,
            string contentType,
            Stream content
        )
        {
            try
            {
                // Ensure the directory exists
                var directory = Path.Combine(
                    _fileStorageSettings.BasePath,
                    _fileStorageSettings.InventoryDocumentsPath
                );

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Generate a unique file name
                var uniqueFileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{fileName}";
                var filePath = Path.Combine(directory, uniqueFileName);

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await content.CopyToAsync(fileStream);
                }

                _logger.LogInformation("File uploaded successfully: {FilePath}", filePath);

                // Return the file URL
                return Path.Combine(
                    _fileStorageSettings.BaseUrl,
                    _fileStorageSettings.InventoryDocumentsPath,
                    uniqueFileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file: {FileName}", fileName);
                throw;
            }
        }

        /// <summary>
        /// Downloads a file from storage.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        /// <returns>A task representing the asynchronous operation, with the file content as the result.</returns>
        public async Task<(Stream Content, string ContentType, string FileName)> DownloadFileAsync(
            string fileUrl
        )
        {
            try
            {
                // Parse the file URL to get the file path
                var baseUrl = _fileStorageSettings.BaseUrl.TrimEnd('/');
                var relativePath = fileUrl.Replace(baseUrl, string.Empty).TrimStart('/');
                var filePath = Path.Combine(_fileStorageSettings.BasePath, relativePath);

                // Check if the file exists
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("File not found: {FilePath}", filePath);
                    throw new FileNotFoundException("The requested file was not found.", filePath);
                }

                // Get the file content type
                var contentType = GetContentType(filePath);

                // Get the file name
                var fileName = Path.GetFileName(filePath);

                // Load the file into a memory stream
                var content = new MemoryStream();
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    await fileStream.CopyToAsync(content);
                }

                // Reset the position of the memory stream
                content.Position = 0;

                _logger.LogInformation("File downloaded successfully: {FilePath}", filePath);

                return (content, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file: {FileUrl}", fileUrl);
                throw;
            }
        }

        /// <summary>
        /// Deletes a file from storage.
        /// </summary>
        /// <param name="fileUrl">The file URL.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task DeleteFileAsync(string fileUrl)
        {
            try
            {
                // Parse the file URL to get the file path
                var baseUrl = _fileStorageSettings.BaseUrl.TrimEnd('/');
                var relativePath = fileUrl.Replace(baseUrl, string.Empty).TrimStart('/');
                var filePath = Path.Combine(_fileStorageSettings.BasePath, relativePath);

                // Check if the file exists
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("File not found for deletion: {FilePath}", filePath);
                    return Task.CompletedTask;
                }

                // Delete the file
                File.Delete(filePath);

                _logger.LogInformation("File deleted successfully: {FilePath}", filePath);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FileUrl}", fileUrl);
                throw;
            }
        }

        private string GetContentType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".doc" => "application/msword",
                ".docx"
                    => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".csv" => "text/csv",
                ".txt" => "text/plain",
                _ => "application/octet-stream"
            };
        }
    }
}
