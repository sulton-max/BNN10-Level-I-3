using LocalIdentity.SimpleInfra.Application.Common.Notifications.Mappers;
using LocalIdentity.SimpleInfra.Domain.Common.Events;
using LocalIdentity.SimpleInfra.Domain.Enums;
using Newtonsoft.Json;

namespace LocalIdentity.SimpleInfra.Application.Common.Notifications.Events;

[JsonConverter(typeof(NotificationEventConverter))]
public abstract class NotificationEvent : Event
{
    public Guid SenderUserId { get; init; }

    public Guid ReceiverUserId { get; init; }

    public override Type GetEventType(string eventTypeDiscriminator)
    {
        var eventType = Enum.Parse<NotificationProcessingEvent>(eventTypeDiscriminator);

        return eventType switch
        {
            NotificationProcessingEvent.OnProcessing => typeof(ProcessNotificationEvent),
            NotificationProcessingEvent.OnRendering => typeof(RenderNotificationEvent),
            NotificationProcessingEvent.OnSending => typeof(SendNotificationEvent),
            _ => throw new JsonException($"Unable to find valid configuration for provided \"type\". TypeValue: {eventType}.")
        };
    }
}