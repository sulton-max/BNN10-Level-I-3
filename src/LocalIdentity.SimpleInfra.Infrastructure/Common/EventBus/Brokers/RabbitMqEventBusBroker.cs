using System.Text;
using LocalIdentity.SimpleInfra.Application.Common.EventBus.Brokers;
using LocalIdentity.SimpleInfra.Application.Common.Serialization;
using LocalIdentity.SimpleInfra.Domain.Common.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.EventBus.Brokers;

public class RabbitMqEventBusBroker(
    IRabbitMqConnectionProvider rabbitMqConnectionProvider,
    IJsonSerializationSettingsProvider jsonSerializationSettingsProvider
) : IEventBusBroker
{
    public async ValueTask PublishAsync<TEvent>(TEvent @event, string exchange, string routingKey, CancellationToken cancellationToken)
        where TEvent : Event
    {
        var channel = await rabbitMqConnectionProvider.CreateChannelAsync();

        var properties = new BasicProperties
        {
            Persistent = true
        };

        var serializerSettings = jsonSerializationSettingsProvider.Get(true);
        // serializerSettings.Converters = [new EventConverter()];

        // var test = new JsonSerializerSettings
        // {
        //     PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        //     //Formatting = Formatting.Indented,
        //     ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        //     //ContractResolver = new CamelCasePropertyNamesContractResolver(),
        //     //NullValueHandling = NullValueHandling.Ignore
        // };
        //
        // var tesB = JsonConvert.SerializeObject(@event, test);

        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event, serializerSettings));
        await channel.BasicPublishAsync(exchange, routingKey, properties, body);
    }
}