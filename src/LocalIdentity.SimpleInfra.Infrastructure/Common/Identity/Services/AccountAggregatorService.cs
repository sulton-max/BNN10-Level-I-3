using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;
using LocalIdentity.SimpleInfra.Application.Common.Notfications.Services;
using LocalIdentity.SimpleInfra.Application.Common.Verifications.Services;
using LocalIdentity.SimpleInfra.Domain.Constants;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class AccountAggregatorService(
    IUserService userService,
    IUserInfoVerificationCodeService userInfoVerificationCodeService,
    IEmailOrchestrationService emailOrchestrationService
) : IAccountAggregatorService
{
    public async ValueTask<bool> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        // create user
        var createdUser = await userService.CreateAsync(user, cancellationToken: cancellationToken);

        // send welcome email
        var systemUser = await userService.GetSystemUserAsync(cancellationToken: cancellationToken);
        await emailOrchestrationService.SendAsync(
            new EmailNotificationRequest
            {
                SenderUserId = systemUser.Id,
                ReceiverUserId = createdUser.Id,
                TemplateType = NotificationTemplateType.WelcomeNotification,
                Variables = new Dictionary<string, string>
                {
                    { NotificationTemplateConstants.UserNamePlaceholder, createdUser.FirstName }
                }
            },
            cancellationToken
        );

        // send verification email
        var verificationCode = await userInfoVerificationCodeService.CreateAsync(
            VerificationCodeType.EmailAddressVerification,
            createdUser.Id,
            cancellationToken
        );

        await emailOrchestrationService.SendAsync(
            new EmailNotificationRequest
            {
                SenderUserId = systemUser.Id,
                ReceiverUserId = createdUser.Id,
                TemplateType = NotificationTemplateType.EmailAddressVerificationNotification,
                Variables = new Dictionary<string, string>
                {
                    { NotificationTemplateConstants.EmailAddressVerificationLinkPlaceholder, verificationCode.VerificationLink }
                }
            },
            cancellationToken
        );

        return true;
    }
}