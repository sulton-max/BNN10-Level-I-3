using System.Text;
using LocalIdentity.SimpleInfra.Application.Common.EventBus.Brokers;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.EventBus.Services;

public abstract class AsyncEventBusSubscriber : IEventSubscriber
{
    private readonly IRabbitMqConnectionProvider _rabbitMqConnectionProvider;
    private EventingBasicConsumer _consumer;
    private IChannel _channel;
    private readonly EventBusSubscriberSettings _eventBusSubscriberSettings;

    public AsyncEventBusSubscriber(
        IRabbitMqConnectionProvider rabbitMqConnectionProvider,
        IOptions<EventBusSubscriberSettings> eventBusSubscriberSettings
    )
    {
        _rabbitMqConnectionProvider = rabbitMqConnectionProvider;
        _eventBusSubscriberSettings = eventBusSubscriberSettings.Value;
    }

    public async ValueTask StartAsync(CancellationToken token)
    {
        await SetChannelAsync();
        await SetConsumerAsync(token);
    }

    public ValueTask StopAsync(CancellationToken token)
    {
        _channel.Dispose();
        return ValueTask.CompletedTask;
    }

    protected virtual async ValueTask SetChannelAsync()
    {
        _channel = await _rabbitMqConnectionProvider.CreateChannelAsync();
        await _channel.BasicQosAsync(0, _eventBusSubscriberSettings.PrefetchCount, false);
    }

    protected virtual async ValueTask SetConsumerAsync(CancellationToken cancellationToken)
    {
        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Received += async (sender, args) => await HandleInternalAsync(sender, args, cancellationToken);
        await _channel.BasicConsumeAsync(_eventBusSubscriberSettings.QueueName, false, _consumer);
    }

    protected virtual async ValueTask HandleInternalAsync(object? sender, BasicDeliverEventArgs ea, CancellationToken cancellationToken)
    {
        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
        var result = await ProcessAsync(message, cancellationToken);

        if (result.Result)
            await _channel.BasicAckAsync(ea.DeliveryTag, false);
        else
            await _channel.BasicNackAsync(ea.DeliveryTag, false, result.Redeliver);
    }

    protected abstract ValueTask<(bool Result, bool Redeliver)> ProcessAsync(string message, CancellationToken cancellationToken);
}