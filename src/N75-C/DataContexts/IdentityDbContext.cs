using Microsoft.EntityFrameworkCore;
using N75_C.Models;
using N75_C.Models.Common;
using N75_C.Models.Entities;

namespace N75_C.DataContexts;

public class IdentityDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<EmailNotificationEvent> EmailNotificationEvents => Set<EmailNotificationEvent>();

    public IdentityDbContext() : base(new DbContextOptionsBuilder<IdentityDbContext>()
        .UseNpgsql("Host=localhost;Port=5432;Database=CrudExample;Username=postgres;Password=postgres")
        .Options)
    {
    }

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }
}