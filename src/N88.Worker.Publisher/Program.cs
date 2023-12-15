using System.Text;
using N88.Common;
using Newtonsoft.Json;
using RabbitMQ.Client;

// create connection factory
var connectionFactory = new ConnectionFactory
{
    HostName = "localhost"
};

// create connection
var connection = await connectionFactory.CreateConnectionAsync();

// create channel
var channel = await connection.CreateChannelAsync();

// create exchange
channel.ExchangeDeclare(exchange: MessagingConstants.IdentityExchange, type: ExchangeType.Fanout, durable: true);

// create queue
await channel.QueueDeclareAsync(queue: MessagingConstants.VerificationProcessingQueue, durable: true, exclusive: false, autoDelete: false);

await channel.QueueDeclareAsync(queue: MessagingConstants.ResumeGenerationQueue, durable: true, exclusive: false, autoDelete: false);

await channel.QueueDeclareAsync(queue: MessagingConstants.FeedGenerationQueue, durable: true, exclusive: false, autoDelete: false);

// bind queue
await channel.QueueBindAsync(
    exchange: MessagingConstants.IdentityExchange,
    queue: MessagingConstants.VerificationProcessingQueue,
    routingKey: string.Empty
);

await channel.QueueBindAsync(
    exchange: MessagingConstants.IdentityExchange,
    queue: MessagingConstants.ResumeGenerationQueue,
    routingKey: string.Empty
    );

await channel.QueueBindAsync(
    exchange: MessagingConstants.IdentityExchange,
    queue: MessagingConstants.FeedGenerationQueue,
    routingKey: string.Empty
);

// produce message
var userCreatedEvent = new UserCreatedEvent(Guid.NewGuid());
var message = JsonConvert.SerializeObject(userCreatedEvent);
var body = Encoding.UTF8.GetBytes(message);

// publish message
await channel.BasicPublishAsync(exchange: MessagingConstants.IdentityExchange, routingKey: MessagingConstants.IdentityQueue, body: body);