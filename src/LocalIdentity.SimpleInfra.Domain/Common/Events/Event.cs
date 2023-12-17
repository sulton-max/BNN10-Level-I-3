namespace LocalIdentity.SimpleInfra.Domain.Common.Events;

public abstract class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.Now;

    public uint RetryAttemptsCount { get; set; }

    /// <summary>
    /// For serialization
    /// </summary>
    public abstract string EventTypeDiscriminator { get; }

    /// <summary>
    ///  For deserialization
    /// </summary>
    public abstract Type GetEventType(string eventTypeDiscriminator);
}