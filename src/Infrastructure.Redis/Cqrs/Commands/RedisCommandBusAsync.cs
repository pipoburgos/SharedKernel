using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Infrastructure.Requests;
using SharedKernel.Infrastructure.Requests.Middlewares;
using StackExchange.Redis;

namespace SharedKernel.Infrastructure.Redis.Cqrs.Commands;

/// <summary>  </summary>
internal class RedisCommandBusAsync : ICommandBusAsync
{
    private readonly IExecuteMiddlewaresService _executeMiddlewaresService;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IRequestSerializer _requestSerializer;

    /// <summary>  </summary>
    public RedisCommandBusAsync(
        IExecuteMiddlewaresService executeMiddlewaresService,
        IConnectionMultiplexer connectionMultiplexer,
        IRequestSerializer requestSerializer)
    {
        _executeMiddlewaresService = executeMiddlewaresService;
        _connectionMultiplexer = connectionMultiplexer;
        _requestSerializer = requestSerializer;
    }

    /// <summary>  </summary>
    public Task Dispatch(CommandRequest command, CancellationToken cancellationToken)
    {
        return _executeMiddlewaresService.ExecuteAsync(command, cancellationToken, (req, _) =>
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
