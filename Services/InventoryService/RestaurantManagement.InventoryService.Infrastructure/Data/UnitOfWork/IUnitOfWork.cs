using RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IInventoryItemRepository InventoryItems { get; }
        IStockLevelRepository StockLevels { get; }
        ILocationRepository Locations { get; }
        IInventoryTransactionRepository InventoryTransactions { get; }
        IBatchRepository Batches { get; }
        IVendorRepository Vendors { get; }
        ICountSheetRepository CountSheets { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
