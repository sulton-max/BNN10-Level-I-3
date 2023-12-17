using System.Text;
using LocalIdentity.SimpleInfra.Application.Common.EventBus.Brokers;
using LocalIdentity.SimpleInfra.Application.Common.Notifications.Mappers;
using LocalIdentity.SimpleInfra.Application.Common.Serialization;
using LocalIdentity.SimpleInfra.Domain.Common.Events;
using LocalIdentity.SimpleInfra.Domain.Extensions;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.EventBus.Services;

public abstract class EventSubscriber<TEvent> : IEventSubscriber where TEvent : Event
{
    private readonly EventBusSubscriberSettings _eventBusSubscriberSettings;
    private IEnumerable<EventingBasicConsumer> _consumers = default!;
    private readonly JsonSerializerSettings _jsonSerializerSettings;
    private readonly IRabbitMqConnectionProvider _rabbitMqConnectionProvider;
    private readonly IEnumerable<string> _queueNames;

    protected IChannel Channel = default!;

    protected EventSubscriber(
        IRabbitMqConnectionProvider rabbitMqConnectionProvider,
        IOptions<EventBusSubscriberSettings> eventBusSubscriberSettings,
        IEnumerable<string> queueNames,
        IJsonSerializationSettingsProvider jsonSerializationSettingsProvider
    )
    {
        _rabbitMqConnectionProvider = rabbitMqConnectionProvider;
        _eventBusSubscriberSettings = eventBusSubscriberSettings.Value;
        _queueNames = queueNames;

        // Set the serializer settings
        _jsonSerializerSettings = jsonSerializationSettingsProvider.Get(true);
        // _jsonSerializerSettings.Converters = [new EventConverter()];
        // _jsonSerializerSettings.Converters = typeof(TEvent).GetJsonConverters();
    }

    public async ValueTask StartAsync(CancellationToken token)
    {
        await SetChannelAsync();
        await SetConsumerAsync(token);
    }

    public ValueTask StopAsync(CancellationToken token)
    {
        Channel.Dispose();
        return ValueTask.CompletedTask;
    }

    protected virtual async ValueTask SetChannelAsync()
    {
        Channel = await _rabbitMqConnectionProvider.CreateChannelAsync();
        await Channel.BasicQosAsync(0, _eventBusSubscriberSettings.PrefetchCount, false);
    }

    protected virtual async ValueTask SetConsumerAsync(CancellationToken cancellationToken)
    {
        _consumers = await Task.WhenAll(
            _queueNames.Select(
                async queueName =>
                {
                    var consumer = new EventingBasicConsumer(Channel);
                    consumer.Received += async (sender, args) => await HandleInternalAsync(sender, args, cancellationToken);
                    await Channel.BasicConsumeAsync(queueName, false, consumer);

                    return consumer;
                }
            )
        );
    }

    protected virtual async ValueTask HandleInternalAsync(object? sender, BasicDeliverEventArgs ea, CancellationToken cancellationToken)
    {
        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
        var @event = (TEvent)JsonConvert.DeserializeObject(message, typeof(TEvent), _jsonSerializerSettings)!;
        var result = await ProcessAsync(@event, cancellationToken);

        if (result.Result)
            await Channel.BasicAckAsync(ea.DeliveryTag, false);
        else
            await Channel.BasicNackAsync(ea.DeliveryTag, false, result.Redeliver);
    }

    protected abstract ValueTask<(bool Result, bool Redeliver)> ProcessAsync(TEvent @event, CancellationToken cancellationToken);
}