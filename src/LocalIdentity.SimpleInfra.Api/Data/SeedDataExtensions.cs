using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;
using LocalIdentity.SimpleInfra.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace LocalIdentity.SimpleInfra.Api.Data;

public static class SeedDataExtensions
{
    public static async ValueTask InitializeSeedAsync(this IServiceProvider serviceProvider)
    {
        var identityDbContext = serviceProvider.GetRequiredService<IdentityDbContext>();
        var notificationDbContext = serviceProvider.GetRequiredService<NotificationDbContext>();
        var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();

        if (!await identityDbContext.Users.AnyAsync())
            await identityDbContext.SeedUsersAsync();

        if (!await notificationDbContext.EmailTemplates.AnyAsync())
            await notificationDbContext.SeedEmailTemplates(webHostEnvironment);

        if (notificationDbContext.ChangeTracker.HasChanges())
            await notificationDbContext.SaveChangesAsync();
    }

    private static async ValueTask SeedUsersAsync(this IdentityDbContext identityDbContext)
    {
        if (await identityDbContext.Users.FirstOrDefaultAsync(user => user.Role == RoleType.System) is not null)
            return;

        await identityDbContext.Users.AddAsync(
            new User
            {
                Id = Guid.Parse("B852F637-1779-48FE-9ADD-EA6BCE4068DE"),
                FirstName = "System",
                LastName = string.Empty,
                Age = 0,
                EmailAddress = "sultonbek.rakhimov.recovery@gmail.com",
                PasswordHash = string.Empty,
                Role = RoleType.System
            }
        );
        await identityDbContext.SaveChangesAsync();
    }

    private static async ValueTask SeedEmailTemplates(this NotificationDbContext notificationDbContext, IWebHostEnvironment webHostEnvironment)
    {
        var emailTemplateTypes = new List<NotificationTemplateType>
        {
            NotificationTemplateType.WelcomeNotification,
            NotificationTemplateType.EmailAddressVerificationNotification,
            NotificationTemplateType.ReferralNotification
        };

        var emailTemplateContents = await Task.WhenAll(
            emailTemplateTypes.Select(
                async templateType =>
                {
                    var filePath = Path.Combine(
                        webHostEnvironment.ContentRootPath,
                        "Data",
                        "EmailTemplates",
                        Path.ChangeExtension(templateType.ToString(), "html")
                    );
                    return (TemplateType: templateType, TemplateContent: await File.ReadAllTextAsync(filePath));
                }
            )
        );

        var emailTemplates = emailTemplateContents.Select(
            templateContent => templateContent.TemplateType switch
            {
                NotificationTemplateType.WelcomeNotification => new EmailTemplate
                {
                    TemplateType = templateContent.TemplateType,
                    Subject = "Welcome to our service!",
                    Content = templateContent.TemplateContent
                },
                NotificationTemplateType.EmailAddressVerificationNotification => new EmailTemplate
                {
                    TemplateType = templateContent.TemplateType,
                    Subject = "Confirm your email address",
                    Content = templateContent.TemplateContent
                },
                NotificationTemplateType.ReferralNotification => new EmailTemplate
                {
                    TemplateType = templateContent.TemplateType,
                    Subject = "You have been referred!",
                    Content = templateContent.TemplateContent
                },
                _ => throw new NotSupportedException("Template type not supported.")
            }
        );

        await notificationDbContext.EmailTemplates.AddRangeAsync(emailTemplates);
    }
}