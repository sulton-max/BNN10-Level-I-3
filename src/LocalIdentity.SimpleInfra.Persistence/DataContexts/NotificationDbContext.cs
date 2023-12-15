using LocalIdentity.SimpleInfra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Persistence.DataContexts;

public class NotificationDbContext : DbContext
{
    public DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();

    public DbSet<EmailHistory> EmailHistories => Set<EmailHistory>();

    public DbSet<User> Users => Set<User>();

    public DbSet<UserSettings> UserSettings => Set<UserSettings>();

    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);
    }
}