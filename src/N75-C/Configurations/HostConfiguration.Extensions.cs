using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using N75_C.DataContexts;
using N75_C.Models.Settings;
using N75_C.Services;

namespace N75_C.Configurations;

public static partial class HostConfiguration
{
    private static WebApplicationBuilder AddNotificationsInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<SmtpEmailSenderSettings>(builder.Configuration.GetSection(nameof(SmtpEmailSenderSettings)));

        builder.Services.Configure<NotificationSenderSettings>(builder.Configuration.GetSection(nameof(NotificationSenderSettings)));

        builder.Services.AddScoped<EmailSenderService>();

        // registering hosted services
        builder.Services.AddHostedService<EmailSenderBackgroundService>(
            provider =>
            {
                var scopedService = provider.CreateScope();

                return new EmailSenderBackgroundService(
                    scopedService.ServiceProvider.GetRequiredService<IOptions<NotificationSenderSettings>>(),
                    scopedService.ServiceProvider.GetRequiredService<EmailSenderService>()
                );
            }
        );

        return builder;
    }

    private static WebApplicationBuilder AddIdentityInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<IdentityDbContext>(
            options => options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityConnection"))
        );

        builder.Services.AddScoped<UserService>().AddScoped<AccountAggregatorService>();

        return builder;
    }

    private static WebApplicationBuilder AddExposers(this WebApplicationBuilder builder)
    {
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddControllers();

        return builder;
    }

    private static WebApplicationBuilder AddDevTools(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }

    private static WebApplication UseExposers(this WebApplication app)
    {
        app.MapControllers();

        return app;
    }

    private static WebApplication UseDevTools(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}