using LocalIdentity.SimpleInfra.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalIdentity.SimpleInfra.Persistence.EntityConfigurations;

public class UserSignInDetailsConfiguration : IEntityTypeConfiguration<UserSignInDetails>
{
    public void Configure(EntityTypeBuilder<UserSignInDetails> builder)
    {
    }
}