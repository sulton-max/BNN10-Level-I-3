using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalIdentity.SimpleInfra.Persistence.EntityConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
            new Role
            {
                Id = Guid.Parse("0327F1BA-81CF-478F-98D4-04FEC56FC10A"),
                Type = RoleType.System,
                IsActive = true,
                CreatedTime = DateTimeOffset.Now
            },
            new Role
            {
                Id = Guid.Parse("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1a"),
                Type = RoleType.Admin,
                IsActive = true,
                CreatedTime = DateTimeOffset.Now
            },
            new Role
            {
                Id = Guid.Parse("d0b0d6c0-2b7a-4b1a-9f1a-0b9b6a9a5b1b"),
                Type = RoleType.User,
                IsActive = true,
                CreatedTime = DateTimeOffset.Now
            }
        );
    }
}