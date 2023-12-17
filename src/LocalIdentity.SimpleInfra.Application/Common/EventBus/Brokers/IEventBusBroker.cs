using LocalIdentity.SimpleInfra.Domain.Common.Events;

namespace LocalIdentity.SimpleInfra.Application.Common.EventBus.Brokers;

public interface IEventBusBroker
{
    ValueTask PublishAsync<TEvent>(TEvent @event, string exchange, string routingKey, CancellationToken cancellationToken) where TEvent : Event;
}