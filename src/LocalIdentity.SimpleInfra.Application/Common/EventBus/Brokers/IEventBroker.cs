using LocalIdentity.SimpleInfra.Domain.Common.Events;

namespace LocalIdentity.SimpleInfra.Application.Common.EventBus.Brokers;

public interface IEventBroker
{
    ValueTask PublishAsync<TEvent>(TEvent @event) where TEvent : Event;

    ValueTask SubscribeAsync<TEvent>() where TEvent : Event;
}