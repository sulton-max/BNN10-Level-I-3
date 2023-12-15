using AutoMapper;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;
using LocalIdentity.SimpleInfra.Application.Common.Notfications.Services;
using LocalIdentity.SimpleInfra.Domain.Common.Exceptions;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;
using LocalIdentity.SimpleInfra.Domain.Extensions;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Notifications.Services;

public class NotificationAggregatorService : INotificationAggregatorService
{
    private readonly IMapper _mapper;
    private readonly IOptions<NotificationSettings> _notificationSettings;
    private readonly IEmailOrchestrationService _emailOrchestrationService;
    private readonly IUserService _userService;
    private readonly IEmailTemplateService _emailTemplateService;

    public NotificationAggregatorService(
        IMapper mapper,
        IOptions<NotificationSettings> notificationSettings,
        IEmailTemplateService emailTemplateService,
        IEmailOrchestrationService emailOrchestrationService,
        IUserService userService
    )
    {
        _mapper = mapper;
        _notificationSettings = notificationSettings;
        _emailOrchestrationService = emailOrchestrationService;
        _emailTemplateService = emailTemplateService;
        _userService = userService;
    }

    public async ValueTask<FuncResult<bool>> SendAsync(NotificationRequest notificationRequest, CancellationToken cancellationToken = default)
    {
        var sendNotificationTask = async () =>
        {
            // If sender is not specified, get system user
            var senderUser = notificationRequest.SenderUserId.HasValue
                ? await _userService.GetByIdAsync(notificationRequest.SenderUserId.Value, cancellationToken: cancellationToken)
                : await _userService.GetSystemUserAsync(true, cancellationToken);

            notificationRequest.SenderUserId = senderUser!.Id;

            var receiverUser = await _userService.GetByIdAsync(notificationRequest.ReceiverUserId, cancellationToken: cancellationToken);

            // If notification provider type is not specified, get from receiver user settings
            if (!notificationRequest.Type.HasValue && receiverUser!.UserSettings.PreferredNotificationType.HasValue)
                notificationRequest.Type = receiverUser!.UserSettings.PreferredNotificationType!.Value;

            // If user not specified preferred notification type get from settings
            if (!notificationRequest.Type.HasValue)
                notificationRequest.Type = _notificationSettings.Value.DefaultNotificationType;

            var sendNotificationTask = notificationRequest.Type switch
            {
                NotificationType.Email => _emailOrchestrationService.SendAsync(
                    _mapper.Map<EmailNotificationRequest>(notificationRequest),
                    cancellationToken
                ),
                _ => throw new NotImplementedException("This type of notification is not supported yet.")
            };

            var sendNotificationResult = await sendNotificationTask;
            return sendNotificationResult.Data;
        };

        return await sendNotificationTask.GetValueAsync();
    }

    public async ValueTask<IList<NotificationTemplate>> GetTemplatesByFilterAsync(
        NotificationTemplateFilter filter,
        CancellationToken cancellationToken = default
    )
    {
        var templates = new List<NotificationTemplate>();

        if (filter.TemplateType.Contains(NotificationType.Email))
            templates.AddRange(await _emailTemplateService.GetByFilterAsync(filter, cancellationToken: cancellationToken));

        return templates;
    }
}