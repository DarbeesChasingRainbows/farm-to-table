using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.EntityConfigurations
{
    public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name).IsRequired().HasMaxLength(100);

            builder.Property(i => i.Description).HasMaxLength(500);

            builder.Property(i => i.SKU).IsRequired().HasMaxLength(50);

            builder.HasIndex(i => i.SKU).IsUnique();

            builder.Property(i => i.Category).IsRequired().HasMaxLength(50);

            builder.Property(i => i.Subcategory).HasMaxLength(50);

            builder.Property(i => i.UnitOfMeasure).IsRequired().HasMaxLength(20);

            builder.Property(i => i.StorageRequirements).HasMaxLength(200);

            builder.Property(i => i.ReorderThreshold).IsRequired();

            builder.Property(i => i.MinStockLevel);

            builder.Property(i => i.MaxStockLevel);

            builder.Property(i => i.LeadTimeDays);

            builder.Property(i => i.TrackExpiration).IsRequired().HasDefaultValue(false);

            builder.Property(i => i.DefaultVendorId).HasMaxLength(36);

            builder.Property(i => i.CostingMethod).HasMaxLength(20);

            builder.Property(i => i.LastCost).HasPrecision(18, 2);

            builder.Property(i => i.AverageCost).HasPrecision(18, 2);

            builder.Property(i => i.IsActive).IsRequired().HasDefaultValue(true);

            builder.Property(i => i.CreatedAt).IsRequired();

            builder.Property(i => i.CreatedBy).IsRequired().HasMaxLength(50);

            builder.Property(i => i.LastModifiedAt);

            builder.Property(i => i.LastModifiedBy).HasMaxLength(50);

            // Many-to-many relationship with alternative items (self-referencing)
            builder
                .HasMany(i => i.AlternativeItems)
                .WithMany()
                .UsingEntity(
                    "InventoryItemAlternatives",
                    l =>
                        l.HasOne<InventoryItem>()
                            .WithMany()
                            .HasForeignKey("InventoryItemId")
                            .OnDelete(DeleteBehavior.Restrict),
                    r =>
                        r.HasOne<InventoryItem>()
                            .WithMany()
                            .HasForeignKey("AlternativeItemId")
                            .OnDelete(DeleteBehavior.Restrict)
                );

            // One-to-many relationship with stock levels
            builder
                .HasMany(i => i.StockLevels)
                .WithOne(s => s.InventoryItem)
                .HasForeignKey(s => s.InventoryItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many relationship with batches
            builder
                .HasMany(i => i.Batches)
                .WithOne(b => b.InventoryItem)
                .HasForeignKey(b => b.InventoryItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
