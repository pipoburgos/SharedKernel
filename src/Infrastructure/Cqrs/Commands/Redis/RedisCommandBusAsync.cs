using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Infrastructure.Requests;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Cqrs.Commands.Redis;

/// <summary>  </summary>
public class RedisCommandBusAsync : ICommandBusAsync
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IRequestSerializer _requestSerializer;

    /// <summary>  </summary>
    public RedisCommandBusAsync(
        IConnectionMultiplexer connectionMultiplexer,
        IRequestSerializer requestSerializer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _requestSerializer = requestSerializer;
    }

    /// <summary>  </summary>
    public Task Dispatch(CommandRequest command, CancellationToken cancellationToken)
    {
        var eventAsString = _requestSerializer.Serialize(command);
        return _connectionMultiplexer.GetDatabase()
            .ListRightPushAsync("CommandsQueue", eventAsString);
    }

    /// <summary>  </summary>
    public Task Dispatch(IEnumerable<CommandRequest> commands, CancellationToken cancellationToken)
    {
        return Task.WhenAll(commands.Select(@event => Dispatch(@event, cancellationToken)));
    }
}

