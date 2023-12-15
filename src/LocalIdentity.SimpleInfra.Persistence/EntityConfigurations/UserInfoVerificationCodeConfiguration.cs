using LocalIdentity.SimpleInfra.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalIdentity.SimpleInfra.Persistence.EntityConfigurations;

public class UserInfoVerificationCodeConfiguration : IEntityTypeConfiguration<UserInfoVerificationCode>
{
    public void Configure(EntityTypeBuilder<UserInfoVerificationCode> builder)
    {
        builder.HasOne<User>().WithMany().HasForeignKey(code => code.UserId);
    }
}