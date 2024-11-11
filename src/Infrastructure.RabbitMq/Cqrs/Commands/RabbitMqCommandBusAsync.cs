using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.RabbitMq.Cqrs.Commands;

/// <summary> . </summary>
public class RabbitMqCommandBusAsync : RabbitMqPublisher, ICommandBusAsync
{
    private readonly IRequestSerializer _requestSerializer;
    private readonly IPipeline _pipeline;

    /// <summary> . </summary>
    public RabbitMqCommandBusAsync(
        IRequestSerializer requestSerializer,
        IPipeline pipeline,
        ILogger<RabbitMqPublisher> logger,
        RabbitMqConnectionFactory config,
        IOptions<RabbitMqConfigParams> rabbitMqParams) : base(logger, config, rabbitMqParams)
    {
        _requestSerializer = requestSerializer;
        _pipeline = pipeline;
    }

    /// <summary> . </summary>
    public Task Dispatch(CommandRequest command, CancellationToken cancellationToken)
    {
        return _pipeline.ExecuteAsync(command, cancellationToken, (req, _) =>
        {
            var serializedDomainEvent = _requestSerializer.Serialize(req);

            return PublishOnQueue(serializedDomainEvent, command.GetUniqueName(), cancellationToken);
        });
    }

    /// <summary> . </summary>
    public Task Dispatch(IEnumerable<CommandRequest> commands, CancellationToken cancellationToken)
    {
        return Task.WhenAll(commands.Select(command => Dispatch(command, cancellationToken)));
    }
}
