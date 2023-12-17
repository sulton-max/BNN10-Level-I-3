using LocalIdentity.SimpleInfra.Domain.Common.Events;
using LocalIdentity.SimpleInfra.Domain.Enums;
using LocalIdentity.SimpleInfra.Domain.Serializers;
using Newtonsoft.Json;

namespace LocalIdentity.SimpleInfra.Application.Common.Notifications.Events;

// [JsonConverter(typeof(JsonTypeConverter<NotificationEvent>))]
public class NotificationEvent : Event
{
    public Guid SenderUserId { get; init; }

    public Guid ReceiverUserId { get; init; }

    public override string GetTypeDiscriminator() =>
        throw new NotSupportedException($"{typeof(NotificationEvent)} doesn't support type discriminator.");

    public override Type ResolveType(string typeName)
    {
        var eventType = Enum.Parse<NotificationProcessingEvent>(typeName);

        return eventType switch
        {
            NotificationProcessingEvent.OnProcessing => typeof(ProcessNotificationEvent),
            NotificationProcessingEvent.OnRendering => typeof(RenderNotificationEvent),
            NotificationProcessingEvent.OnSending => typeof(SendNotificationEvent),
            _ => throw new JsonException($"Unable to find valid configuration for provided \"type\". TypeValue: {eventType}.")
        };
    }
}