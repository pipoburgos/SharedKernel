using Microsoft.Extensions.Options;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.ActiveMq.Events;

/// <summary> . </summary>
public class ActiveMqEventBus : ActiveMqPublisher, IEventBus
{
    private readonly IRequestSerializer _requestSerializer;
    private readonly IPipeline _pipeline;

    /// <summary> . </summary>
    public ActiveMqEventBus(
        IRequestSerializer requestSerializer,
        IPipeline pipeline,
        IOptions<ActiveMqConfiguration> configuration) : base(configuration)
    {
        _requestSerializer = requestSerializer;
        _pipeline = pipeline;
    }

    /// <summary> . </summary>
    public Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
    {
        return Task.WhenAll(events.Select(@event => Publish(@event, cancellationToken)));
    }

    /// <summary> . </summary>
    public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
    {
        return _pipeline.ExecuteAsync(@event, cancellationToken, (req, _) =>
        {
            var serializedDomainEvent = _requestSerializer.Serialize(req);

            return PublishTopic(serializedDomainEvent, @event.GetEventName());
        });
    }
}

