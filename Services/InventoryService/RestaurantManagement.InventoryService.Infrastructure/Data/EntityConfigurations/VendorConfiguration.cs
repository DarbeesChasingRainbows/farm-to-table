using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.EntityConfigurations
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Name).IsRequired().HasMaxLength(100);

            builder.HasIndex(v => v.Name).IsUnique();

            builder.Property(v => v.ContactName).IsRequired().HasMaxLength(100);

            builder.Property(v => v.Email).IsRequired().HasMaxLength(100);

            builder.Property(v => v.Phone).IsRequired().HasMaxLength(20);

            builder.Property(v => v.Street).HasMaxLength(200);

            builder.Property(v => v.City).HasMaxLength(100);

            builder.Property(v => v.State).HasMaxLength(50);

            builder.Property(v => v.PostalCode).HasMaxLength(20);

            builder.Property(v => v.Country).HasMaxLength(50);

            builder.Property(v => v.AccountNumber).HasMaxLength(50);

            builder.Property(v => v.PaymentTerms).HasMaxLength(100);

            builder.Property(v => v.Notes).HasMaxLength(500);

            builder.Property(v => v.IsActive).IsRequired().HasDefaultValue(true);

            builder.Property(v => v.CreatedAt).IsRequired();

            builder.Property(v => v.CreatedBy).IsRequired().HasMaxLength(50);

            builder.Property(v => v.LastModifiedAt);

            builder.Property(v => v.LastModifiedBy).HasMaxLength(50);

            // Configure the VendorItem owned entity type
            builder.OwnsMany(
                v => v.SuppliedItems,
                itemBuilder =>
                {
                    itemBuilder.WithOwner().HasForeignKey("VendorId");

                    itemBuilder.Property<string>("Id").IsRequired().HasMaxLength(36);

                    itemBuilder.HasKey("Id");

                    itemBuilder.Property(i => i.ItemId).IsRequired().HasMaxLength(36);

                    itemBuilder.Property(i => i.VendorSku).HasMaxLength(50);

                    itemBuilder.Property(i => i.UnitCost).IsRequired().HasPrecision(18, 2);

                    itemBuilder.Property(i => i.UnitOfMeasure).IsRequired().HasMaxLength(20);

                    itemBuilder.Property(i => i.MinOrderQuantity).HasPrecision(18, 3);

                    itemBuilder.Property(i => i.LeadTimeDays).IsRequired();

                    itemBuilder.Property(i => i.IsPreferred).IsRequired().HasDefaultValue(false);

                    // Create a unique index on VendorId and ItemId
                    itemBuilder.HasIndex("VendorId", "ItemId").IsUnique();

                    // Navigation property to InventoryItem
                    itemBuilder
                        .HasOne(i => i.Item)
                        .WithMany()
                        .HasForeignKey(i => i.ItemId)
                        .OnDelete(DeleteBehavior.Restrict);
                }
            );
        }
    }
}
