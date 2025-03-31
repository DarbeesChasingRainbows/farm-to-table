using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces
{
    public interface ICountSheetRepository : IRepository<CountSheet>
    {
        Task<IReadOnlyList<CountSheet>> GetByLocationIdAsync(string locationId);
        Task<IReadOnlyList<CountSheet>> GetByStatusAsync(string status);
        Task<IReadOnlyList<CountSheet>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IReadOnlyList<CountSheet>> GetByRequestedByAsync(string userId);
        Task<IReadOnlyList<CountSheet>> GetByCountedByAsync(string userId);
        Task<IReadOnlyList<CountSheet>> GetByApprovedByAsync(string userId);
        Task<CountSheet?> GetWithItemsAsync(string countSheetId);
        Task<CountSheetItem?> GetCountSheetItemAsync(string countSheetId, string itemId);
        Task AddCategoryAsync(string countSheetId, string category);
        Task AddItemAsync(
            string countSheetId,
            string itemId,
            string itemName,
            string sku,
            string category,
            string unitOfMeasure,
            double systemQuantity
        );
        Task RecordCountAsync(
            string countSheetId,
            string itemId,
            double countedQuantity,
            string? batchId = null
        );
        Task RecordBatchCountAsync(
            string countSheetId,
            string itemId,
            string batchId,
            double countedQuantity
        );
        Task ApproveVarianceAsync(string countSheetId, string itemId, string reasonCode);
        Task CompleteCountSheetAsync(string countSheetId, DateTime completedDate);
        Task ApproveCountSheetAsync(string countSheetId, string approvedBy, DateTime approvalDate);
    }
}
