using Microsoft.Extensions.Options;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Logging;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.RabbitMq;
using SharedKernel.Infrastructure.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Cqrs.Commands.RabbitMq;

/// <summary>  </summary>
public class RabbitMqCommandBusAsync : RabbitMqPublisher, ICommandBusAsync
{
    private readonly IRequestSerializer _requestSerializer;
    private readonly IExecuteMiddlewaresService _executeMiddlewaresService;

    /// <summary>  </summary>
    public RabbitMqCommandBusAsync(
        IRequestSerializer requestSerializer,
        IExecuteMiddlewaresService executeMiddlewaresService,
        ICustomLogger<RabbitMqPublisher> logger,
        RabbitMqConnectionFactory config,
        IOptions<RabbitMqConfigParams> rabbitMqParams) : base(logger, config, rabbitMqParams)
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

            return PublishOnQueue(serializedDomainEvent, command.GetUniqueName());
        });
    }

    /// <summary>  </summary>
    public Task Dispatch(IEnumerable<CommandRequest> commands, CancellationToken cancellationToken)
    {
        return Task.WhenAll(commands.Select(command => Dispatch(command, cancellationToken)));
    }
}
