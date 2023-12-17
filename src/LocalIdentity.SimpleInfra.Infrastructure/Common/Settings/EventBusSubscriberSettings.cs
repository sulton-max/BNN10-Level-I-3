namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;

public abstract class EventBusSubscriberSettings
{
    public ushort PrefetchCount { get; set; }
}