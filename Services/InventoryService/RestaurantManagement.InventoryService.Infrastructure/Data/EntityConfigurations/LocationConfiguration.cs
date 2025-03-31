using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.EntityConfigurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Name).IsRequired().HasMaxLength(100);

            builder.HasIndex(l => l.Name).IsUnique();

            builder.Property(l => l.Description).HasMaxLength(500);

            builder.Property(l => l.Type).IsRequired().HasMaxLength(50);

            builder.Property(l => l.MinTemperature).HasPrecision(5, 2);

            builder.Property(l => l.MaxTemperature).HasPrecision(5, 2);

            builder.Property(l => l.TargetHumidity).HasPrecision(5, 2);

            builder.Property(l => l.SpecialRequirements).HasMaxLength(200);

            builder.Property(l => l.Capacity).HasPrecision(18, 3);

            builder.Property(l => l.CapacityUnit).HasMaxLength(20);

            builder.Property(l => l.IsActive).IsRequired().HasDefaultValue(true);

            builder.Property(l => l.CreatedAt).IsRequired();

            builder.Property(l => l.CreatedBy).IsRequired().HasMaxLength(50);

            builder.Property(l => l.LastModifiedAt);

            builder.Property(l => l.LastModifiedBy).HasMaxLength(50);
        }
    }
}
