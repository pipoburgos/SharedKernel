using Microsoft.Extensions.Options;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Infrastructure.Requests;
using SharedKernel.Infrastructure.Requests.Middlewares;

namespace SharedKernel.Infrastructure.ActiveMq.Cqrs.Comamnds;

/// <summary>  </summary>
public class ActiveMqCommandBusAsync : ActiveMqPublisher, ICommandBusAsync
{
    private readonly IRequestSerializer _requestSerializer;
    private readonly IExecuteMiddlewaresService _executeMiddlewaresService;

    /// <summary>  </summary>
    public ActiveMqCommandBusAsync(
        IRequestSerializer requestSerializer,
        IExecuteMiddlewaresService executeMiddlewaresService,
        IOptions<ActiveMqConfiguration> configuration) : base(configuration)
    {
        _requestSerializer = requestSerializer;
        _executeMiddlewaresService = executeMiddlewaresService;
    }

    /// <summary>  </summary>
    public Task Dispatch(CommandRequest command, CancellationToken cancellationToken)
    {
        return _executeMiddlewaresService.ExecuteAsync(command, cancellationToken, (req, _) =>
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
