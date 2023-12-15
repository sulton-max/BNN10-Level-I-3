using LocalIdentity.SimpleInfra.Application.Common.EventBus.Brokers;
using LocalIdentity.SimpleInfra.Domain.Common.Events;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.EventBus.Brokers;

public class RabbitMqEventBroker : IEventBroker
{
    public ValueTask PublishAsync<TEvent>(TEvent @event) where TEvent : Event
    {
        throw new NotImplementedException();
    }

    public ValueTask SubscribeAsync<TEvent>() where TEvent : Event
    {
        throw new NotImplementedException();
    }
}