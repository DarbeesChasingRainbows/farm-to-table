using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.EntityConfigurations
{
    public class StockLevelConfiguration : IEntityTypeConfiguration<StockLevel>
    {
        public void Configure(EntityTypeBuilder<StockLevel> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.InventoryItemId).IsRequired().HasMaxLength(36);

            builder.Property(s => s.LocationId).IsRequired().HasMaxLength(36);

            builder.Property(s => s.CurrentQuantity).IsRequired().HasPrecision(18, 3);

            builder
                .Property(s => s.ReservedQuantity)
                .IsRequired()
                .HasPrecision(18, 3)
                .HasDefaultValue(0);

            builder.Property(s => s.LastUpdated).IsRequired();

            // Create a unique index on InventoryItemId and LocationId
            builder.HasIndex(s => new { s.InventoryItemId, s.LocationId }).IsUnique();

            // Navigation property to InventoryItem
            builder
                .HasOne(s => s.InventoryItem)
                .WithMany(i => i.StockLevels)
                .HasForeignKey(s => s.InventoryItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add a computed column for AvailableQuantity
            builder
                .Property(s => s.AvailableQuantity)
                .HasComputedColumnSql("[CurrentQuantity] - [ReservedQuantity]", stored: true);
        }
    }
}
