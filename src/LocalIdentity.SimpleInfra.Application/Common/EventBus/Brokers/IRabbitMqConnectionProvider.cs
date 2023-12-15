using RabbitMQ.Client;

namespace LocalIdentity.SimpleInfra.Application.Common.EventBus.Brokers;

public interface IRabbitMqConnectionProvider
{
    ValueTask<IChannel> CreateChannelAsync();
}