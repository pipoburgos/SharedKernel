using Microsoft.Extensions.Options;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Requests;
using SharedKernel.Infrastructure.Requests.Middlewares;

namespace SharedKernel.Infrastructure.ActiveMq.Events;

/// <summary>  </summary>
public class ActiveMqEventBus : ActiveMqPublisher, IEventBus
{
    private readonly IRequestSerializer _requestSerializer;
    private readonly IExecuteMiddlewaresService _executeMiddlewaresService;

    /// <summary>  </summary>
    public ActiveMqEventBus(
        IRequestSerializer requestSerializer,
        IExecuteMiddlewaresService executeMiddlewaresService,
        IOptions<ActiveMqConfiguration> configuration) : base(configuration)
    {
        _requestSerializer = requestSerializer;
        _executeMiddlewaresService = executeMiddlewaresService;
    }

    /// <summary>  </summary>
    public Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
    {
        return Task.WhenAll(events.Select(@event => Publish(@event, cancellationToken)));
    }

    /// <summary>  </summary>
    public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
    {
        return _executeMiddlewaresService.ExecuteAsync(@event, cancellationToken, (req, _) =>
        {
            var serializedDomainEvent = _requestSerializer.Serialize(req);

            return PublishTopic(serializedDomainEvent, @event.GetEventName());
        });
    }
}

