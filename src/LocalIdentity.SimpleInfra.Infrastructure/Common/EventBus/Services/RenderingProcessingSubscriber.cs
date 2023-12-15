using LocalIdentity.SimpleInfra.Application.Common.EventBus.Brokers;
using LocalIdentity.SimpleInfra.Application.Common.Notfications.Events;
using LocalIdentity.SimpleInfra.Application.Common.Notfications.Services;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.EventBus.Services;

public class RenderingProcessingSubscriber(
    IRabbitMqConnectionProvider rabbitMqConnectionProvider,
    IOptions<EventBusSubscriberSettings> eventBusSubscriberSettings,
    IEmailRenderingService emailRenderingService
) : AsyncEventBusSubscriber(rabbitMqConnectionProvider, eventBusSubscriberSettings)
{
    protected override ValueTask<(bool Result, bool Redeliver)> ProcessAsync(string message, CancellationToken cancellationToken)
    {
        var renderingProcessingEvent =
            JsonConvert.DeserializeObject<RenderNotificationEvent>(message) ?? throw new ArgumentNullException(nameof(message));

        // renderingProcessingEvent.NotificationContext
        // message.Template = await _emailTemplateService.GetByTypeAsync(request.TemplateType, true, cancellationToken) ??
        //                    throw new InvalidOperationException($"Invalid template for sending {NotificationType.Email} notification");
        //
        // emailRenderingService.RenderAsync(renderingProcessingEvent.NotificationContext, cancellationToken);
    }
}