using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Api.Configurations;

public static class MigrationExtensions
{
    public static async ValueTask MigrateAsync<TContext>(this IServiceScopeFactory scopeFactory) where TContext : DbContext
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
            await context.Database.MigrateAsync();
    }
}