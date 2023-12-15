using AirBnb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirBnb.Persistence.EntityConfigurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder
            .HasOne<Location>()
            .WithMany(location => location.Cities)
            .HasForeignKey(location => location.ParentId);

        builder.Property(location => location.Name).IsRequired().HasMaxLength(256);
        builder.Property(location => location.Code).IsRequired(false).HasMaxLength(64);
    }
}