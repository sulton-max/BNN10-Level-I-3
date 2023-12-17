using LocalIdentity.SimpleInfra.Application.Common.EventBus.Brokers;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.EventBus.Brokers;

public class RabbitMqConnectionProvider : IRabbitMqConnectionProvider
{
    private readonly ConnectionFactory _connectionFactory;

    private IConnection? _connection;

    public RabbitMqConnectionProvider(IOptions<RabbitMqConnectionSettings> rabbitMqConnectionSettings)
    {
        var rabbitMqConnectionSettings1 = rabbitMqConnectionSettings.Value;

        _connectionFactory = new ConnectionFactory
        {
            HostName = rabbitMqConnectionSettings1.HostName,
            Port = rabbitMqConnectionSettings1.Port
        };
    }

    public async ValueTask<IChannel> CreateChannelAsync()
    {
        _connection ??= await _connectionFactory.CreateConnectionAsync();

        return await _connection.CreateChannelAsync();
    }
}