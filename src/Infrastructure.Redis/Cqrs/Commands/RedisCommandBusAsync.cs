using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Requests;
using StackExchange.Redis;

namespace SharedKernel.Infrastructure.Redis.Cqrs.Commands;

/// <summary>  </summary>
internal class RedisCommandBusAsync : ICommandBusAsync
{
    private readonly IPipeline _pipeline;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IConfiguration _configuration;
    private readonly IRequestSerializer _requestSerializer;

    /// <summary>  </summary>
    public RedisCommandBusAsync(
        IPipeline pipeline,
        IConnectionMultiplexer connectionMultiplexer,
        IConfiguration configuration,
        IRequestSerializer requestSerializer)
    {
        _pipeline = pipeline;
        _connectionMultiplexer = connectionMultiplexer;
        _configuration = configuration;
        _requestSerializer = requestSerializer;
    }

    /// <summary>  </summary>
    public Task Dispatch(CommandRequest command, CancellationToken cancellationToken)
    {
        var publishQueue = _configuration.GetValue<string?>("RabbitMq:PublishQueue") ?? "CommandsQueue";

        return _pipeline.ExecuteAsync(command, cancellationToken, (req, _) =>
        {
            var eventAsString = _requestSerializer.Serialize(req);
            return _connectionMultiplexer.GetDatabase()
                .ListRightPushAsync(publishQueue, eventAsString);
        });
    }

    /// <summary>  </summary>
    public Task Dispatch(IEnumerable<CommandRequest> commands, CancellationToken cancellationToken)
    {
        return Task.WhenAll(commands.Select(@event => Dispatch(@event, cancellationToken)));
    }
}
