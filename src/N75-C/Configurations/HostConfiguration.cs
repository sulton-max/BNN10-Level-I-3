namespace N75_C.Configurations;

public static partial class HostConfiguration
{
    public static ValueTask<WebApplicationBuilder> ConfigureAsync(this WebApplicationBuilder builder)
    {
        builder.AddNotificationsInfrastructure().AddIdentityInfrastructure().AddDevTools().AddExposers();

        return new ValueTask<WebApplicationBuilder>(builder);
    }

    public static ValueTask<WebApplication> ConfigureAsync(this WebApplication app)
    {
        app.UseDevTools().UseExposers();

        return new ValueTask<WebApplication>(app);
    }
}