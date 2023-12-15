using LocalIdentity.SimpleInfra.Application.Common.EventBus.Brokers;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.EventBus.Brokers;

public class RabbitMqConnectionProvider : IRabbitMqConnectionProvider
{
    private readonly RabbitMqConnectionSettings _rabbitMqConnectionSettings;

    private ConnectionFactory _connectionFactory;

    private IConnection? _connection;

    public RabbitMqConnectionProvider(IOptions<RabbitMqConnectionSettings> rabbitMqConnectionSettings)
    {
        _rabbitMqConnectionSettings = rabbitMqConnectionSettings.Value;

        _connectionFactory = new ConnectionFactory
        {
            HostName = _rabbitMqConnectionSettings.HostName,
            Port = _rabbitMqConnectionSettings.Port
        };
    }

    public async ValueTask<IChannel> CreateChannelAsync()
    {
        _connection ??= await _connectionFactory.CreateConnectionAsync();

        return await _connection.CreateChannelAsync();
    }
}