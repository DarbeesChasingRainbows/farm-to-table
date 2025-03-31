using Microsoft.EntityFrameworkCore.Storage;
using RestaurantManagement.InventoryService.Infrastructure.Data.Context;
using RestaurantManagement.InventoryService.Infrastructure.Data.Repositories;
using RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InventoryDbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed;

        public IInventoryItemRepository InventoryItems { get; }
        public IStockLevelRepository StockLevels { get; }
        public ILocationRepository Locations { get; }
        public IInventoryTransactionRepository InventoryTransactions { get; }
        public IBatchRepository Batches { get; }
        public IVendorRepository Vendors { get; }
        public ICountSheetRepository CountSheets { get; }

        public UnitOfWork(
            InventoryDbContext context,
            IInventoryItemRepository inventoryItemRepository,
            IStockLevelRepository stockLevelRepository,
            ILocationRepository locationRepository,
            IInventoryTransactionRepository inventoryTransactionRepository,
            IBatchRepository batchRepository,
            IVendorRepository vendorRepository,
            ICountSheetRepository countSheetRepository
        )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            InventoryItems =
                inventoryItemRepository
                ?? throw new ArgumentNullException(nameof(inventoryItemRepository));
            StockLevels =
                stockLevelRepository
                ?? throw new ArgumentNullException(nameof(stockLevelRepository));
            Locations =
                locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));
            InventoryTransactions =
                inventoryTransactionRepository
                ?? throw new ArgumentNullException(nameof(inventoryTransactionRepository));
            Batches = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
            Vendors = vendorRepository ?? throw new ArgumentNullException(nameof(vendorRepository));
            CountSheets =
                countSheetRepository
                ?? throw new ArgumentNullException(nameof(countSheetRepository));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();

                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
                _transaction?.Dispose();
            }
            _disposed = true;
        }
    }
}
