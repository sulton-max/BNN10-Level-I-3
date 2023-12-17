using AutoMapper;
using FluentValidation;
using LocalIdentity.SimpleInfra.Application.Common.EventBus.Brokers;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Application.Common.Notifications.Events;
using LocalIdentity.SimpleInfra.Application.Common.Notifications.Models;
using LocalIdentity.SimpleInfra.Application.Common.Notifications.Services;
using LocalIdentity.SimpleInfra.Application.Common.Serialization;
using LocalIdentity.SimpleInfra.Domain.Constants;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;
using LocalIdentity.SimpleInfra.Domain.Extensions;
using LocalIdentity.SimpleInfra.Infrastructure.Common.EventBus.Services;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Notifications.EventSubscriber;

public class NotificationSubscriber(
    IServiceScopeFactory serviceScopeFactory,
    IMapper mapper,
    IJsonSerializationSettingsProvider jsonSerializationSettingsProvider,
    IOptions<NotificationSubscriberSettings> eventBusSubscriberSettings,
    IRabbitMqConnectionProvider rabbitMqConnectionProvider,
    IEventBusBroker eventBusBroker,
    IOptions<NotificationSettings> notificationSettings
) : EventSubscriber<NotificationEvent>(
    rabbitMqConnectionProvider,
    eventBusSubscriberSettings,
    [EventBusConstants.ProcessNotificationQueueName, EventBusConstants.RenderNotificationQueueName, EventBusConstants.SendNotificationQueueName],
    jsonSerializationSettingsProvider
)
{
    private readonly NotificationSettings _notificationSettings = notificationSettings.Value;

    protected override async ValueTask SetChannelAsync()
    {
        await base.SetChannelAsync();

        await Channel.ExchangeDeclareAsync(EventBusConstants.NotificationExchangeName, type: ExchangeType.Direct, durable: true);

        await Channel.QueueDeclareAsync(EventBusConstants.ProcessNotificationQueueName, true, false, false);
        await Channel.QueueDeclareAsync(EventBusConstants.RenderNotificationQueueName, true, false, false);
        await Channel.QueueDeclareAsync(EventBusConstants.SendNotificationQueueName, true, false, false);

        await Channel.QueueBindAsync(
            queue: EventBusConstants.ProcessNotificationQueueName,
            exchange: EventBusConstants.NotificationExchangeName,
            routingKey: EventBusConstants.ProcessNotificationQueueName
        );

        await Channel.QueueBindAsync(
            queue: EventBusConstants.RenderNotificationQueueName,
            exchange: EventBusConstants.NotificationExchangeName,
            routingKey: EventBusConstants.RenderNotificationQueueName
        );

        await Channel.QueueBindAsync(
            queue: EventBusConstants.SendNotificationQueueName,
            exchange: EventBusConstants.NotificationExchangeName,
            routingKey: EventBusConstants.SendNotificationQueueName
        );
    }

    protected override async ValueTask<(bool Result, bool Redeliver)> ProcessAsync(NotificationEvent @event, CancellationToken cancellationToken)
    {
        var eventHandler = () => @event switch
        {
            ProcessNotificationEvent processNotificationEvent => ProcessNotificationAsync(processNotificationEvent, cancellationToken),
            RenderNotificationEvent processNotificationEvent => RenderNotificationAsync(processNotificationEvent, cancellationToken),
            SendNotificationEvent processNotificationEvent => SendNotificationAsync(processNotificationEvent, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(@event))
        };

        // declare exchange and queues
        var result = await eventHandler.GetValueAsync();
        return (result.Data, Redeliver: false);
    }

    private async ValueTask ProcessNotificationAsync(ProcessNotificationEvent processNotificationEvent, CancellationToken cancellationToken)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var emailTemplateService = scope.ServiceProvider.GetRequiredService<IEmailTemplateService>();
        var processNotificationEventValidator = scope.ServiceProvider.GetRequiredService<IValidator<ProcessNotificationEvent>>();

        var validationResult = await processNotificationEventValidator.ValidateAsync(processNotificationEvent, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var senderUser = processNotificationEvent.SenderUserId != Guid.Empty
            ? await userService.GetByIdAsync(processNotificationEvent.SenderUserId, cancellationToken: cancellationToken)
            : await userService.GetSystemUserAsync(true, cancellationToken);

        // processNotificationEvent.SenderUserId = senderUser!.Id;
        var receiverUser = await userService.GetByIdAsync(processNotificationEvent.ReceiverUserId, cancellationToken: cancellationToken);

        // If notification provider type is not specified, get from receiver user settings
        if (!processNotificationEvent.Type.HasValue && receiverUser!.UserSettings.PreferredNotificationType.HasValue)
            processNotificationEvent.Type = receiverUser!.UserSettings.PreferredNotificationType!.Value;

        // If user not specified preferred notification type get from settings
        if (!processNotificationEvent.Type.HasValue)
            processNotificationEvent.Type = _notificationSettings.DefaultNotificationType;

        if (processNotificationEvent.Type == NotificationType.Email)
        {
            var renderNotificationEvent = new RenderNotificationEvent
            {
                SenderUserId = processNotificationEvent.SenderUserId,
                ReceiverUserId = processNotificationEvent.ReceiverUserId,
                Template = (await emailTemplateService.GetByTypeAsync(processNotificationEvent.TemplateType, cancellationToken: cancellationToken))!,
                SenderUser = senderUser!,
                ReceiverUser = (await userService.GetByIdAsync(processNotificationEvent.SenderUserId, cancellationToken: cancellationToken))!,
            };

            await eventBusBroker.PublishAsync(
                renderNotificationEvent,
                EventBusConstants.NotificationExchangeName,
                EventBusConstants.RenderNotificationQueueName,
                cancellationToken
            );
        }
    }

    private async ValueTask RenderNotificationAsync(RenderNotificationEvent renderNotificationEvent, CancellationToken cancellationToken)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var emailRenderingService = scope.ServiceProvider.GetRequiredService<IEmailRenderingService>();

        if (renderNotificationEvent.Template.Type == NotificationType.Email)
        {
            var emailMessage = new EmailMessage
            {
                SenderEmailAddress = renderNotificationEvent.SenderUser.EmailAddress,
                ReceiverEmailAddress = renderNotificationEvent.ReceiverUser.EmailAddress,
                Template = (EmailTemplate)renderNotificationEvent.Template,
                Variables = renderNotificationEvent.Variables
            };

            await emailRenderingService.RenderAsync(emailMessage, cancellationToken);

            var sendNotificationEvent = new SendNotificationEvent
            {
                SenderUserId = renderNotificationEvent.SenderUserId,
                ReceiverUserId = renderNotificationEvent.ReceiverUserId,
                Message = emailMessage
            };

            await eventBusBroker.PublishAsync(
                sendNotificationEvent,
                EventBusConstants.NotificationExchangeName,
                EventBusConstants.SendNotificationQueueName,
                cancellationToken
            );
        }
    }

    private async ValueTask SendNotificationAsync(SendNotificationEvent sendNotificationEvent, CancellationToken cancellationToken)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var emailSenderService = scope.ServiceProvider.GetRequiredService<IEmailSenderService>();
        var emailHistoryService = scope.ServiceProvider.GetRequiredService<IEmailHistoryService>();

        if (sendNotificationEvent.Message is EmailMessage emailMessage)
        {
            await emailSenderService.SendAsync(emailMessage, cancellationToken);

            var history = mapper.Map<EmailHistory>(emailMessage);
            await emailHistoryService.CreateAsync(history, cancellationToken: cancellationToken);

            if (history.IsSuccessful) throw new InvalidOperationException("Email history is not created");
        }
    }
}