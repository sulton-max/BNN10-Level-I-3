using AirBnb.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirBnb.Persistence.DataContexts;

public class AppDbContext : DbContext
{
    public DbSet<Location> Locations => Set<Location>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}