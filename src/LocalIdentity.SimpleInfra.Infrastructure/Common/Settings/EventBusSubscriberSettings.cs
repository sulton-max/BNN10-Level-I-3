namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;

public class EventBusSubscriberSettings
{
    public ushort PrefetchCount { get; set; }

    public string QueueName { get; set; } = default!;
}