using LocalIdentity.SimpleInfra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Persistence.DataContexts;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<UserInfoVerificationCode> UserInfoVerificationCodes => Set<UserInfoVerificationCode>();
    
    public DbSet<UserSignInDetails> UserSignInDetails => Set<UserSignInDetails>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }
}