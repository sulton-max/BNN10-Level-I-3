namespace LocalIdentity.SimpleInfra.Api.Configurations;

public static partial class HostConfiguration
{
    public static ValueTask<WebApplicationBuilder> ConfigureAsync(this WebApplicationBuilder builder)
    {
        builder.AddSerializers()
            .AddMappers()
            .AddValidators()
            .AddCaching()
            .AddEventBus()
            .AddNotificationInfrastructure()
            .AddVerificationInfrastructure()
            .AddIdentityInfrastructure()
            .AddDevTools()
            .AddExposers();

        return new ValueTask<WebApplicationBuilder>(builder);
    }

    public static async ValueTask<WebApplication> ConfigureAsync(this WebApplication app)
    {
        await app.MigrateDatabaseAsync();
        await app.SeedDataAsync();

        app.UseIdentityInfrastructure().UseDevTools().UseExposers();

        return app;
    }
}