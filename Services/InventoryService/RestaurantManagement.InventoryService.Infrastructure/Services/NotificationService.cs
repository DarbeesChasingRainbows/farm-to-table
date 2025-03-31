using System.Text.Json;
using Microsoft.Extensions.Logging;
using RestaurantManagement.InventoryService.Application.Interfaces;
using RestaurantManagement.InventoryService.Application.Models;

namespace RestaurantManagement.InventoryService.Infrastructure.Services
{
    /// <summary>
    /// Implementation of the notification service.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<NotificationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="emailService">The email service.</param>
        /// <param name="logger">The logger.</param>
        public NotificationService(IEmailService emailService, ILogger<NotificationService> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Sends a notification.
        /// </summary>
        /// <param name="notification">The notification to send.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendNotificationAsync(Notification notification)
        {
            try
            {
                _logger.LogInformation(
                    "Sending notification of type {NotificationType} to {Recipients}",
                    notification.NotificationType,
                    string.Join(", ", notification.Recipients)
                );

                switch (notification.NotificationType)
                {
                    case NotificationType.Email:
                        await SendEmailNotificationAsync(notification);
                        break;

                    case NotificationType.Push:
                        await SendPushNotificationAsync(notification);
                        break;

                    case NotificationType.Sms:
                        await SendSmsNotificationAsync(notification);
                        break;

                    default:
                        _logger.LogWarning(
                            "Unsupported notification type: {NotificationType}",
                            notification.NotificationType
                        );
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error sending notification of type {NotificationType} to {Recipients}",
                    notification.NotificationType,
                    string.Join(", ", notification.Recipients)
                );
                throw;
            }
        }

        private async Task SendEmailNotificationAsync(Notification notification)
        {
            var emailMessage = new EmailMessage
            {
                Subject = notification.Subject,
                Body = notification.Message,
                ToAddresses = notification.Recipients,
                IsHtml = notification.IsHtml
            };

            await _emailService.SendEmailAsync(emailMessage);
        }

        private Task SendPushNotificationAsync(Notification notification)
        {
            // This would be implemented with a push notification service
            // For now, we'll just log it
            _logger.LogInformation(
                "Push notification would be sent: {Notification}",
                JsonSerializer.Serialize(notification)
            );

            return Task.CompletedTask;
        }

        private Task SendSmsNotificationAsync(Notification notification)
        {
            // This would be implemented with an SMS service
            // For now, we'll just log it
            _logger.LogInformation(
                "SMS notification would be sent: {Notification}",
                JsonSerializer.Serialize(notification)
            );

            return Task.CompletedTask;
        }
    }
}
