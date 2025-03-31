using RestaurantManagement.InventoryService.Application.Interfaces;

namespace RestaurantManagement.InventoryService.Infrastructure.Services
{
    /// <summary>
    /// Implementation of the date time service.
    /// </summary>
    public class DateTimeService : IDateTimeService
    {
        /// <summary>
        /// Gets the current UTC date and time.
        /// </summary>
        public DateTime Now => DateTime.UtcNow;
    }
}
