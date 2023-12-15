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
var verificationProcessingSubscriber = new EventingBasicConsumer(channel);
var resumeGenerationSubscriber = new EventingBasicConsumer(channel);
var feedGenerationSubscriber = new EventingBasicConsumer(channel);

// handle received message
verificationProcessingSubscriber.Received += async (sender, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    // process message
    var identityEvent = JsonConvert.DeserializeObject<UserCreatedEvent>(message)!;
    Console.WriteLine($"Verification message sent to user by Id : {identityEvent.UserId}");

    // acknowledge message
    await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
};

// handle received message
resumeGenerationSubscriber.Received += async (sender, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    // process message
    var identityEvent = JsonConvert.DeserializeObject<UserCreatedEvent>(message)!;
    Console.WriteLine($"Resume generated for user Id: {identityEvent.UserId}");

    // acknowledge message
    await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
};

// handle received message
feedGenerationSubscriber.Received += async (sender, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    // process message
    var identityEvent = JsonConvert.DeserializeObject<UserCreatedEvent>(message)!;
    Console.WriteLine($"Feed generated for user Id: {identityEvent.UserId}");

    // acknowledge message
    await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
};

channel.BasicConsume(queue: MessagingConstants.VerificationProcessingQueue, autoAck: false, consumer: verificationProcessingSubscriber);
channel.BasicConsume(queue: MessagingConstants.ResumeGenerationQueue, autoAck: false, consumer: resumeGenerationSubscriber);
channel.BasicConsume(queue: MessagingConstants.FeedGenerationQueue, autoAck: false, consumer: feedGenerationSubscriber);

Console.ReadLine();