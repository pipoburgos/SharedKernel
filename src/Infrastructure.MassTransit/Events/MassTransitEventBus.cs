using MassTransit;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.MassTransit.Cqrs.Commands;
using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.MassTransit.Events;

/// <summary>  </summary>
public class MassTransitEventBus : IEventBus
{
    private readonly IRequestSerializer _requestSerializer;
    private readonly IPipeline _pipeline;
    private readonly IPublishEndpoint _publishEndpoint;

    /// <summary>  </summary>
    public MassTransitEventBus(
        IRequestSerializer requestSerializer,
        IPipeline pipeline,
        IPublishEndpoint publishEndpoint)
    {
        _requestSerializer = requestSerializer;
        _pipeline = pipeline;
        _publishEndpoint = publishEndpoint;
    }

    /// <summary>  </summary>
    public Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
    {
        return Task.WhenAll(events.Select(@event => Publish(@event, cancellationToken)));
    }

    /// <summary>  </summary>
    public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
    {
        return _pipeline.ExecuteAsync(@event, cancellationToken, (req, _) =>
        {
            var serializedDomainEvent = _requestSerializer.Serialize(req);

            return _publishEndpoint.Publish(new MassTransitCommand { Content = serializedDomainEvent }, cancellationToken);
        });
    }
}
