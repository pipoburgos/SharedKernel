using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace SharedKernel.Infrastructure.RabbitMq;

/// <summary> . </summary>
public class RabbitMqConnectionFactory
{
    private readonly ConnectionFactory _connectionFactory;

    /// <summary> . </summary>
    public RabbitMqConnectionFactory(RabbitMqConfigParams rabbitMqParams)
    {
        var configParams = rabbitMqParams;

        _connectionFactory = new ConnectionFactory
        {
            HostName = configParams.HostName,
            UserName = configParams.Username,
            Password = configParams.Password,
            Port = configParams.Port,
        };
    }

    /// <summary> . </summary>
    public RabbitMqConnectionFactory(IOptions<RabbitMqConfigParams> rabbitMqParams)
    {
        var configParams = rabbitMqParams.Value;

        _connectionFactory = new ConnectionFactory
        {
            HostName = configParams.HostName,
            UserName = configParams.Username,
            Password = configParams.Password,
            Port = configParams.Port,
        };
    }

    /// <summary> . </summary>
    public Task<IConnection> CreateConnectionAsync()
    {
        return _connectionFactory.CreateConnectionAsync();
    }
}