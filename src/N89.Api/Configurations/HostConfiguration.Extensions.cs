using System.Reflection;
using AirBnb.Api.Data;
using AirBnb.Application.Locations.Services;
using AirBnb.Infrastructure.Common.Caching.Brokers;
using AirBnb.Infrastructure.Common.Settings;
using AirBnb.Infrastructure.Locations.Services;
using AirBnb.Persistence.Caching.Brokers;
using AirBnb.Persistence.DataContexts;
using AirBnb.Persistence.Repositories;
using AirBnb.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirBnb.Api.Configurations;

public static partial class HostConfiguration
{
    private static readonly ICollection<Assembly> Assemblies;

    static HostConfiguration()
    {
        Assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();
        Assemblies.Add(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Adds mappers
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <returns>The <see cref="WebApplicationBuilder"/> instance.</returns>
    private static WebApplicationBuilder AddMappers(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(Assemblies);

        return builder;
    }

    /// <summary>
    /// Adds caching
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <returns>The <see cref="WebApplicationBuilder"/> instance.</returns>
    private static WebApplicationBuilder AddCaching(this WebApplicationBuilder builder)
    {
        // Register cache settings
        builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection(nameof(CacheSettings)));

        // Register redis cache
        builder.Services.AddStackExchangeRedisCache(
            options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
                options.InstanceName = "Caching.SimpleInfra";
            }
        );

        // Register cache broker
        builder.Services.AddSingleton<ICacheBroker, RedisDistributedCacheBroker>();

        return builder;
    }

    /// <summary>
    /// Adds business logic infrastructure
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <returns>The <see cref="WebApplicationBuilder"/> instance.</returns>
    private static WebApplicationBuilder AddBusinessLogicInfrastructure(this WebApplicationBuilder builder)
    {
        // register db contexts
        builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        # region Locations

        // register repositories
        builder.Services.AddScoped<ILocationRepository, LocationRepository>();

        // register foundation data access services
        builder.Services.AddScoped<ILocationService, LocationService>();

        #endregion

        return builder;
    }

    /// <summary>
    /// Adds route and controller
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance.</param>
    /// <returns>The <see cref="WebApplicationBuilder"/> instance.</returns>
    private static WebApplicationBuilder AddExposers(this WebApplicationBuilder builder)
    {
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddControllers();

        return builder;
    }

    /// <summary>
    /// Configures the middleware to seed data
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    /// <returns>The <see cref="WebApplication"/> instance.</returns>
    private static async Task<WebApplication> SeedDataAsync(this WebApplication app)
    {
        var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        await scope.ServiceProvider.SeedDataAsync();

        return app;
    }

    /// <summary>
    /// Configures the middleware to use exposers.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    /// <returns>The <see cref="WebApplication"/> instance.</returns>
    private static WebApplication UseExposers(this WebApplication app)
    {
        app.MapControllers();

        return app;
    }
}