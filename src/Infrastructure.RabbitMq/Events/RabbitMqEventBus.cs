using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.RabbitMq.Events;

/// <summary> . </summary>
public class RabbitMqEventBus : RabbitMqPublisher, IEventBus
{
    private readonly IRequestSerializer _requestSerializer;
    private readonly IPipeline _pipeline;

    /// <summary> . </summary>
    public RabbitMqEventBus(
        ILogger<RabbitMqPublisher> logger,
        IRequestSerializer requestSerializer,
        RabbitMqConnectionFactory config,
        IPipeline pipeline,
        IOptions<RabbitMqConfigParams> rabbitMqParams) : base(logger, config, rabbitMqParams)
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

            return PublishTopic(serializedDomainEvent, req.GetEventName());
        });
    }
}
