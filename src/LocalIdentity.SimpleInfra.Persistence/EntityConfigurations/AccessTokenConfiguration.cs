using LocalIdentity.SimpleInfra.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalIdentity.SimpleInfra.Persistence.EntityConfigurations;

public class AccessTokenConfiguration : IEntityTypeConfiguration<AccessToken>
{
    public void Configure(EntityTypeBuilder<AccessToken> builder)
    {
        builder.Property(accessToken => accessToken.Token).IsRequired().HasMaxLength(1024);

        builder.HasOne<User>().WithMany().HasForeignKey(accessToken => accessToken.UserId);
    }
}