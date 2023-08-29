using Microsoft.Extensions.Options;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.ActiveMq.Cqrs.Comamnds;

/// <summary>  </summary>
public class ActiveMqCommandBusAsync : ActiveMqPublisher, ICommandBusAsync
{
    private readonly IRequestSerializer _requestSerializer;
    private readonly IPipeline _pipeline;

    /// <summary>  </summary>
    public ActiveMqCommandBusAsync(
        IRequestSerializer requestSerializer,
        IPipeline pipeline,
        IOptions<ActiveMqConfiguration> configuration) : base(configuration)
    {
        _requestSerializer = requestSerializer;
        _pipeline = pipeline;
    }

    /// <summary>  </summary>
    public Task Dispatch(CommandRequest command, CancellationToken cancellationToken)
    {
        return _pipeline.ExecuteAsync(command, cancellationToken, (req, _) =>
        {
            var serializedDomainEvent = _requestSerializer.Serialize(req);

            return PublishOnQueue(serializedDomainEvent);
        });
    }

    /// <summary>  </summary>
    public Task Dispatch(IEnumerable<CommandRequest> commands, CancellationToken cancellationToken)
    {
        return Task.WhenAll(commands.Select(command => Dispatch(command, cancellationToken)));
    }
}
