using LocalIdentity.SimpleInfra.Application.Common.EventBus.Brokers;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.EventBus.Brokers;

public class RabbitMqConnectionProvider(IOptions<RabbitMqConnectionSettings> rabbitMqConnectionSettings) : IRabbitMqConnectionProvider
{
    private readonly ConnectionFactory _connectionFactory = new()
    {
        HostName = rabbitMqConnectionSettings.Value.HostName,
        Port = rabbitMqConnectionSettings.Value.Port
    };

    private IConnection? _connection;

    public async ValueTask<IChannel> CreateChannelAsync()
    {
        _connection ??= await _connectionFactory.CreateConnectionAsync();

        return await _connection.CreateChannelAsync();
    }
}