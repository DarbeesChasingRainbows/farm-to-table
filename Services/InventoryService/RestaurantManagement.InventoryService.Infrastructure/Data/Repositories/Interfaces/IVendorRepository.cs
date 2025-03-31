using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces
{
    public interface IVendorRepository : IRepository<Vendor>
    {
        Task<Vendor?> GetByNameAsync(string name);
        Task<IReadOnlyList<Vendor>> GetActiveVendorsAsync();
        Task<bool> IsNameUniqueAsync(string name, string? excludeVendorId = null);
        Task<IReadOnlyList<Vendor>> SearchVendorsAsync(string searchTerm);
        Task<IReadOnlyList<Vendor>> GetVendorsForItemAsync(string itemId);
        Task<Vendor?> GetWithSuppliedItemsAsync(string vendorId);
        Task<VendorItem?> GetVendorItemAsync(string vendorId, string itemId);
        Task AddSuppliedItemAsync(
            string vendorId,
            string itemId,
            string vendorSku,
            decimal unitCost,
            string unitOfMeasure,
            double? minOrderQuantity,
            int leadTimeDays,
            bool isPreferred
        );
        Task UpdateSuppliedItemAsync(
            string vendorId,
            string itemId,
            string vendorSku,
            decimal unitCost,
            string unitOfMeasure,
            double? minOrderQuantity,
            int leadTimeDays,
            bool isPreferred
        );
        Task RemoveSuppliedItemAsync(string vendorId, string itemId);
        Task<IReadOnlyList<Vendor>> GetPreferredVendorsAsync();
    }
}
