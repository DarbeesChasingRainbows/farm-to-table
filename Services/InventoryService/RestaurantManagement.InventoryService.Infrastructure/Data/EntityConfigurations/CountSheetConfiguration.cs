using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.EntityConfigurations
{
    public class CountSheetConfiguration : IEntityTypeConfiguration<CountSheet>
    {
        public void Configure(EntityTypeBuilder<CountSheet> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.LocationId).IsRequired().HasMaxLength(36);

            builder.Property(c => c.Status).IsRequired().HasMaxLength(20);

            builder.Property(c => c.CountDate).IsRequired();

            builder.Property(c => c.RequestedBy).IsRequired().HasMaxLength(50);

            builder.Property(c => c.CountedBy).HasMaxLength(50);

            builder.Property(c => c.CompletedDate);

            builder.Property(c => c.ApprovedBy).HasMaxLength(50);

            builder.Property(c => c.ApprovalDate);

            builder.Property(c => c.Notes).HasMaxLength(500);

            builder.Property(c => c.CreatedAt).IsRequired();

            // Store categories as a JSON array
            builder
                .Property(c => c.Categories)
                .HasConversion(
                    v => string.Join(";", v),
                    v => v.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                );

            // Configure the CountSheetItem owned entity type
            builder.OwnsMany(
                c => c.Items,
                itemBuilder =>
                {
                    itemBuilder.WithOwner().HasForeignKey("CountSheetId");

                    itemBuilder.Property<string>("Id").IsRequired().HasMaxLength(36);

                    itemBuilder.HasKey("Id");

                    itemBuilder.Property(i => i.ItemId).IsRequired().HasMaxLength(36);

                    itemBuilder.Property(i => i.ItemName).IsRequired().HasMaxLength(100);

                    itemBuilder.Property(i => i.SKU).IsRequired().HasMaxLength(50);

                    itemBuilder.Property(i => i.Category).IsRequired().HasMaxLength(50);

                    itemBuilder.Property(i => i.UnitOfMeasure).IsRequired().HasMaxLength(20);

                    itemBuilder.Property(i => i.SystemQuantity).IsRequired().HasPrecision(18, 3);

                    itemBuilder.Property(i => i.CountedQuantity).HasPrecision(18, 3);

                    itemBuilder.Property(i => i.Variance).HasPrecision(18, 3);

                    itemBuilder.Property(i => i.VariancePercentage).HasPrecision(9, 2);

                    itemBuilder.Property(i => i.UnitCost).HasPrecision(18, 2);

                    itemBuilder.Property(i => i.VarianceValue).HasPrecision(18, 2);

                    itemBuilder.Property(i => i.HasBeenCounted).IsRequired().HasDefaultValue(false);

                    itemBuilder
                        .Property(i => i.VarianceApproved)
                        .IsRequired()
                        .HasDefaultValue(false);

                    itemBuilder.Property(i => i.VarianceReasonCode).HasMaxLength(20);

                    // Configure the BatchCount owned entity type
                    itemBuilder.OwnsMany(
                        i => i.BatchCounts,
                        batchBuilder =>
                        {
                            batchBuilder.WithOwner().HasForeignKey("CountSheetItemId");

                            batchBuilder.Property<string>("Id").IsRequired().HasMaxLength(36);

                            batchBuilder.HasKey("Id");

                            batchBuilder.Property(b => b.BatchId).IsRequired().HasMaxLength(36);

                            batchBuilder
                                .Property(b => b.BatchNumber)
                                .IsRequired()
                                .HasMaxLength(100);

                            batchBuilder.Property(b => b.ExpirationDate).IsRequired();

                            batchBuilder
                                .Property(b => b.SystemQuantity)
                                .IsRequired()
                                .HasPrecision(18, 3);

                            batchBuilder.Property(b => b.CountedQuantity).HasPrecision(18, 3);
                        }
                    );
                }
            );
        }
    }
}
