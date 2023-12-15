namespace LocalIdentity.SimpleInfra.Api.Configurations;

public static partial class HostConfiguration
{
    public static ValueTask<WebApplicationBuilder> ConfigureAsync(this WebApplicationBuilder builder)
    {
        builder
            .AddMappers()
            .AddValidators()
            .AddCaching()
            .AddNotificationInfrastructure()
            .AddVerificationInfrastructure()
            .AddIdentityInfrastructure()
            .AddDevTools()
            .AddExposers();

        return new(builder);
    }

    public static async ValueTask<WebApplication> ConfigureAsync(this WebApplication app)
    {
        await app.SeedDataAsync();

        app
            .UseIdentityInfrastructure()
            .UseDevTools()
            .UseExposers();

        return app;
    }
}