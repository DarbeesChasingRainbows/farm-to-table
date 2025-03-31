using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.EntityConfigurations
{
    public class BatchConfiguration : IEntityTypeConfiguration<Batch>
    {
        public void Configure(EntityTypeBuilder<Batch> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.InventoryItemId).IsRequired().HasMaxLength(36);

            builder.Property(b => b.BatchNumber).IsRequired().HasMaxLength(100);

            builder.Property(b => b.ReceivedDate).IsRequired();

            builder.Property(b => b.ExpirationDate).IsRequired();

            builder.Property(b => b.InitialQuantity).IsRequired().HasPrecision(18, 3);

            builder.Property(b => b.RemainingQuantity).IsRequired().HasPrecision(18, 3);

            builder.Property(b => b.UnitCost).IsRequired().HasPrecision(18, 2);

            builder.Property(b => b.LocationId).IsRequired().HasMaxLength(36);

            builder.Property(b => b.VendorId).HasMaxLength(36);

            builder.Property(b => b.PurchaseOrderId).HasMaxLength(100);

            // Create a unique index on InventoryItemId and BatchNumber
            builder.HasIndex(b => new { b.InventoryItemId, b.BatchNumber }).IsUnique();

            // Create index on expiration date for easier querying
            builder.HasIndex(b => b.ExpirationDate);

            // Navigation property to InventoryItem
            builder
                .HasOne(b => b.InventoryItem)
                .WithMany(i => i.Batches)
                .HasForeignKey(b => b.InventoryItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
