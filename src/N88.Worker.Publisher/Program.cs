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
channel.ExchangeDeclare(MessagingConstants.IdentityExchange, ExchangeType.Fanout, true);

// create queue
await channel.QueueDeclareAsync(MessagingConstants.VerificationProcessingQueue, true, false, false);

await channel.QueueDeclareAsync(MessagingConstants.ResumeGenerationQueue, true, false, false);

await channel.QueueDeclareAsync(MessagingConstants.FeedGenerationQueue, true, false, false);

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

await channel.QueueBindAsync(exchange: MessagingConstants.IdentityExchange, queue: MessagingConstants.FeedGenerationQueue, routingKey: string.Empty);

// produce message
var userCreatedEvent = new UserCreatedEvent(Guid.NewGuid());
var message = JsonConvert.SerializeObject(userCreatedEvent);
var body = Encoding.UTF8.GetBytes(message);

// publish message
await channel.BasicPublishAsync(MessagingConstants.IdentityExchange, MessagingConstants.IdentityQueue, body);