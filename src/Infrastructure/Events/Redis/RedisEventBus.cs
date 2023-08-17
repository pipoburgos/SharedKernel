using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Requests;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.Redis;

/// <summary>  </summary>
public class RedisEventBus : IEventBus
{
    private readonly IExecuteMiddlewaresService _executeMiddlewaresService;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IRequestSerializer _requestSerializer;

    /// <summary>  </summary>
    public RedisEventBus(
        IExecuteMiddlewaresService executeMiddlewaresService,
        IConnectionMultiplexer connectionMultiplexer,
        IRequestSerializer requestSerializer)
    {
        _executeMiddlewaresService = executeMiddlewaresService;
        _connectionMultiplexer = connectionMultiplexer;
        _requestSerializer = requestSerializer;
    }

    /// <summary>  </summary>
    public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
    {
        return _executeMiddlewaresService.ExecuteAsync(@event, cancellationToken, (req, _) =>
        {
            var eventAsString = _requestSerializer.Serialize(req);
            return _connectionMultiplexer.GetSubscriber().PublishAsync(RedisChannel.Pattern("*"), eventAsString);
        });
    }

    /// <summary>  </summary>
    public Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
    {
        return Task.WhenAll(events.Select(@event => Publish(@event, cancellationToken)));
    }
}
