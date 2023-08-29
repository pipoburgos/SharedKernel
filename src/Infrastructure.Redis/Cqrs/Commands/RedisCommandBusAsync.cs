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
    private readonly IRequestSerializer _requestSerializer;

    /// <summary>  </summary>
    public RedisCommandBusAsync(
        IPipeline pipeline,
        IConnectionMultiplexer connectionMultiplexer,
        IRequestSerializer requestSerializer)
    {
        _pipeline = pipeline;
        _connectionMultiplexer = connectionMultiplexer;
        _requestSerializer = requestSerializer;
    }

    /// <summary>  </summary>
    public Task Dispatch(CommandRequest command, CancellationToken cancellationToken)
    {
        return _pipeline.ExecuteAsync(command, cancellationToken, (req, _) =>
        {
            var eventAsString = _requestSerializer.Serialize(req);
            return _connectionMultiplexer.GetDatabase()
                .ListRightPushAsync("CommandsQueue", eventAsString);
        });
    }

    /// <summary>  </summary>
    public Task Dispatch(IEnumerable<CommandRequest> commands, CancellationToken cancellationToken)
    {
        return Task.WhenAll(commands.Select(@event => Dispatch(@event, cancellationToken)));
    }
}
