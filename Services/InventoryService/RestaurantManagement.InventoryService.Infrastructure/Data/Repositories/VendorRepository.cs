using Microsoft.EntityFrameworkCore;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Infrastructure.Data.Context;
using RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories
{
    public class VendorRepository : Repository<Vendor>, IVendorRepository
    {
        public VendorRepository(InventoryDbContext context)
            : base(context) { }

        public async Task<Vendor?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(v => v.Name.ToLower() == name.ToLower());
        }

        public async Task<IReadOnlyList<Vendor>> GetActiveVendorsAsync()
        {
            return await _dbSet.Where(v => v.IsActive).ToListAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name, string? excludeVendorId = null)
        {
            if (string.IsNullOrEmpty(excludeVendorId))
            {
                return !await _dbSet.AnyAsync(v => v.Name.ToLower() == name.ToLower());
            }
            else
            {
                return !await _dbSet.AnyAsync(v =>
                    v.Name.ToLower() == name.ToLower() && v.Id != excludeVendorId
                );
            }
        }

        public async Task<IReadOnlyList<Vendor>> SearchVendorsAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return await _dbSet
                .Where(v =>
                    v.Name.ToLower().Contains(searchTerm)
                    || v.ContactName.ToLower().Contains(searchTerm)
                    || v.Email.ToLower().Contains(searchTerm)
                    || v.Phone.Contains(searchTerm)
                    || (v.City != null && v.City.ToLower().Contains(searchTerm))
                    || (v.Country != null && v.Country.ToLower().Contains(searchTerm))
                )
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Vendor>> GetVendorsForItemAsync(string itemId)
        {
            // Find vendors that supply this item
            var vendorIds = await _context
                .Set<VendorItem>()
                .Where(vi => vi.ItemId == itemId)
                .Select(vi => vi.VendorId)
                .ToListAsync();

            return await _dbSet.Where(v => vendorIds.Contains(v.Id)).ToListAsync();
        }

        public async Task<Vendor?> GetWithSuppliedItemsAsync(string vendorId)
        {
            return await _dbSet
                .Include(v => v.SuppliedItems)
                .ThenInclude(si => si.Item)
                .FirstOrDefaultAsync(v => v.Id == vendorId);
        }

        public async Task<VendorItem?> GetVendorItemAsync(string vendorId, string itemId)
        {
            var vendor = await _dbSet
                .Include(v => v.SuppliedItems.Where(si => si.ItemId == itemId))
                .FirstOrDefaultAsync(v => v.Id == vendorId);

            return vendor?.SuppliedItems.FirstOrDefault(si => si.ItemId == itemId);
        }

        public async Task AddSuppliedItemAsync(
            string vendorId,
            string itemId,
            string vendorSku,
            decimal unitCost,
            string unitOfMeasure,
            double? minOrderQuantity,
            int leadTimeDays,
            bool isPreferred
        )
        {
            var vendor = await _dbSet
                .Include(v => v.SuppliedItems)
                .FirstOrDefaultAsync(v => v.Id == vendorId);

            if (vendor == null)
            {
                throw new ArgumentException($"Vendor with ID {vendorId} not found.");
            }

            // Check if the item already exists
            if (vendor.SuppliedItems.Any(si => si.ItemId == itemId))
            {
                throw new ArgumentException($"Vendor already supplies this item.");
            }

            // Check if the inventory item exists
            var inventoryItem = await _context.Set<InventoryItem>().FindAsync(itemId);
            if (inventoryItem == null)
            {
                throw new ArgumentException($"Inventory item with ID {itemId} not found.");
            }

            // Add the supplied item
            vendor.AddSuppliedItem(
                itemId,
                vendorSku,
                unitCost,
                unitOfMeasure,
                minOrderQuantity,
                leadTimeDays,
                isPreferred
            );
        }

        public async Task UpdateSuppliedItemAsync(
            string vendorId,
            string itemId,
            string vendorSku,
            decimal unitCost,
            string unitOfMeasure,
            double? minOrderQuantity,
            int leadTimeDays,
            bool isPreferred
        )
        {
            var vendor = await _dbSet
                .Include(v => v.SuppliedItems)
                .FirstOrDefaultAsync(v => v.Id == vendorId);

            if (vendor == null)
            {
                throw new ArgumentException($"Vendor with ID {vendorId} not found.");
            }

            // Get the vendor item
            var vendorItem = vendor.SuppliedItems.FirstOrDefault(si => si.ItemId == itemId);
            if (vendorItem == null)
            {
                throw new ArgumentException($"Vendor does not supply the item with ID {itemId}.");
            }

            // Update the vendor item
            vendorItem.Update(
                vendorSku,
                unitCost,
                unitOfMeasure,
                minOrderQuantity,
                leadTimeDays,
                isPreferred
            );
        }

        public async Task RemoveSuppliedItemAsync(string vendorId, string itemId)
        {
            var vendor = await _dbSet
                .Include(v => v.SuppliedItems)
                .FirstOrDefaultAsync(v => v.Id == vendorId);

            if (vendor == null)
            {
                throw new ArgumentException($"Vendor with ID {vendorId} not found.");
            }

            // Remove the supplied item
            vendor.RemoveSuppliedItem(itemId);
        }

        public async Task<IReadOnlyList<Vendor>> GetPreferredVendorsAsync()
        {
            // Find vendors that are marked as preferred for any item
            var vendorIds = await _context
                .Set<VendorItem>()
                .Where(vi => vi.IsPreferred)
                .Select(vi => vi.VendorId)
                .Distinct()
                .ToListAsync();

            return await _dbSet.Where(v => vendorIds.Contains(v.Id)).ToListAsync();
        }

        public override async Task<Vendor?> GetByIdAsync(string id)
        {
            return await _dbSet
                .Include(v => v.SuppliedItems)
                .ThenInclude(si => si.Item)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
