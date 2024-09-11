using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Requests;
using StackExchange.Redis;

namespace SharedKernel.Infrastructure.Redis.Events;

/// <summary> . </summary>
internal class RedisEventBus : IEventBus
{
    private readonly IPipeline _pipeline;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IRequestSerializer _requestSerializer;

    /// <summary> . </summary>
    public RedisEventBus(
        IPipeline pipeline,
        IConnectionMultiplexer connectionMultiplexer,
        IRequestSerializer requestSerializer)
    {
        _pipeline = pipeline;
        _connectionMultiplexer = connectionMultiplexer;
        _requestSerializer = requestSerializer;
    }

    /// <summary> . </summary>
    public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
    {
        return _pipeline.ExecuteAsync(@event, cancellationToken, (req, _) =>
        {
            var eventAsString = _requestSerializer.Serialize(req);
            return _connectionMultiplexer.GetSubscriber().PublishAsync(RedisChannel.Pattern("*"), eventAsString);
        });
    }

    /// <summary> . </summary>
    public Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
    {
        return Task.WhenAll(events.Select(@event => Publish(@event, cancellationToken)));
    }
}
