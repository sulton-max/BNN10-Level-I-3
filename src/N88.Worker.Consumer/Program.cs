using System.Text;
using System.Text.Json;
using N88.Common;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// create connection factory
var connectionFactory = new ConnectionFactory
{
    HostName = "localhost"
};

// create connection
var connection = await connectionFactory.CreateConnectionAsync();

// create channel
var channel = await connection.CreateChannelAsync();

await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

// create consumer
var consumerA = new EventingBasicConsumer(channel);

// handle received message
consumerA.Received += async (sender, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    // process message
    var identityEvent = JsonConvert.DeserializeObject<UserCreatedEvent>(message)!;
    Console.WriteLine($"Verification message sent to user by Id : {identityEvent.UserId}");

    Console.WriteLine($"Resume generated for user Id: {identityEvent.UserId}");

    Console.WriteLine($"Feed generated for user by Id : {identityEvent.UserId}");

    // acknowledge message
    await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
};

channel.BasicConsume(queue: MessagingConstants.IdentityQueue, autoAck: false, consumer: consumerA);

Console.ReadLine();