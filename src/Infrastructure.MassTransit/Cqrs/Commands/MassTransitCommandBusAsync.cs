using MassTransit;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.MassTransit.Cqrs.Commands;

/// <summary>  </summary>
public class MassTransitCommandBusAsync : ICommandBusAsync
{
    private readonly IRequestSerializer _requestSerializer;
    private readonly IPipeline _pipeline;
    private readonly IPublishEndpoint _publishEndpoint;

    /// <summary>  </summary>
    public MassTransitCommandBusAsync(
        IRequestSerializer requestSerializer,
        IPipeline pipeline,
        IPublishEndpoint publishEndpoint)
    {
        _requestSerializer = requestSerializer;
        _pipeline = pipeline;
        _publishEndpoint = publishEndpoint;
    }

    /// <summary>  </summary>
    public Task Dispatch(CommandRequest command, CancellationToken cancellationToken)
    {
        return _pipeline.ExecuteAsync(command, cancellationToken, (req, _) =>
        {
            var serializedDomainEvent = _requestSerializer.Serialize(req);

            return _publishEndpoint.Publish(new MassTransitCommand { Content = serializedDomainEvent }, cancellationToken);
        });
    }

    /// <summary>  </summary>
    public Task Dispatch(IEnumerable<CommandRequest> commands, CancellationToken cancellationToken)
    {
        return Task.WhenAll(commands.Select(command => Dispatch(command, cancellationToken)));
    }
}
