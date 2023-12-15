using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalIdentity.SimpleInfra.Persistence.EntityConfigurations;

public class VerificationCodeConfiguration : IEntityTypeConfiguration<VerificationCode>
{
    public void Configure(EntityTypeBuilder<VerificationCode> builder)
    {
        builder.HasDiscriminator(verificationCode => verificationCode.Type)
            .HasValue<UserInfoVerificationCode>(VerificationType.UserInfoVerificationCode);

        builder.Property(code => code.Code).IsRequired().HasMaxLength(64);
        builder.Property(code => code.VerificationLink).IsRequired().HasMaxLength(256);
    }
}