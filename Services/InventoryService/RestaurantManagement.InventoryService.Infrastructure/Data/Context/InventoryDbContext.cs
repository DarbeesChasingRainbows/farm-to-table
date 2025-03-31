using Microsoft.EntityFrameworkCore;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Infrastructure.Data.EntityConfigurations;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Context
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options) { }

        public DbSet<InventoryItem> InventoryItems { get; set; } = null!;
        public DbSet<StockLevel> StockLevels { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; } = null!;
        public DbSet<Batch> Batches { get; set; } = null!;
        public DbSet<Vendor> Vendors { get; set; } = null!;
        public DbSet<CountSheet> CountSheets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfiguration(new InventoryItemConfiguration());
            modelBuilder.ApplyConfiguration(new StockLevelConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new BatchConfiguration());
            modelBuilder.ApplyConfiguration(new VendorConfiguration());
            modelBuilder.ApplyConfiguration(new CountSheetConfiguration());
        }

        public override int SaveChanges()
        {
            // Add audit information (Created/Modified timestamps)
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Add audit information (Created/Modified timestamps)
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e =>
                    (e.State == EntityState.Added || e.State == EntityState.Modified)
                    && e.Entity is IAuditableEntity
                );

            foreach (var entry in entries)
            {
                var now = DateTime.UtcNow;

                if (entry.Entity is IAuditableEntity auditableEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditableEntity.CreatedAt = now;
                    }
                    else
                    {
                        auditableEntity.LastModifiedAt = now;
                    }
                }
            }
        }
    }

    // Interface for tracking audit fields
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        string CreatedBy { get; set; }
        DateTime? LastModifiedAt { get; set; }
        string? LastModifiedBy { get; set; }
    }
}
