using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.EntityConfigurations
{
    public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
    {
        public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Type).IsRequired().HasMaxLength(50);

            builder.Property(t => t.TransactionDate).IsRequired();

            builder.Property(t => t.ReferenceNumber).HasMaxLength(100);

            builder.Property(t => t.ReferenceType).HasMaxLength(50);

            builder.Property(t => t.LocationId).HasMaxLength(36);

            builder.Property(t => t.DestinationLocationId).HasMaxLength(36);

            builder.Property(t => t.UserId).IsRequired().HasMaxLength(50);

            builder.Property(t => t.Notes).HasMaxLength(500);

            // Index on transaction date for easier querying by date range
            builder.HasIndex(t => t.TransactionDate);

            // Index on reference number and type for easier lookup
            builder
                .HasIndex(t => new { t.ReferenceNumber, t.ReferenceType })
                .HasFilter("ReferenceNumber IS NOT NULL AND ReferenceType IS NOT NULL");

            // Configure the TransactionItem owned entity type
            builder.OwnsMany(
                t => t.Items,
                itemBuilder =>
                {
                    itemBuilder.WithOwner().HasForeignKey("TransactionId");

                    itemBuilder.Property<string>("Id").IsRequired().HasMaxLength(36);

                    itemBuilder.HasKey("Id");

                    itemBuilder.Property(i => i.InventoryItemId).IsRequired().HasMaxLength(36);

                    itemBuilder.Property(i => i.Quantity).IsRequired().HasPrecision(18, 3);

                    itemBuilder.Property(i => i.LocationId).IsRequired().HasMaxLength(36);

                    itemBuilder.Property(i => i.BatchId).HasMaxLength(36);

                    itemBuilder.Property(i => i.UnitCost).HasPrecision(18, 2);

                    // Navigation property to InventoryItem
                    itemBuilder
                        .HasOne(i => i.InventoryItem)
                        .WithMany()
                        .HasForeignKey(i => i.InventoryItemId)
                        .OnDelete(DeleteBehavior.Restrict);
                }
            );
        }
    }
}
