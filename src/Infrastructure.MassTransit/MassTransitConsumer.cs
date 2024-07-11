using MassTransit;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Infrastructure.MassTransit.Cqrs.Commands;
using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.MassTransit;

/// <summary>  </summary>
public class MassTransitConsumer : IConsumer<MassTransitCommand>
{
    private readonly IRequestMediator _requestMediator;

    /// <summary>  </summary>
    /// <param name="requestMediator"></param>
    public MassTransitConsumer(IRequestMediator requestMediator)
    {
        _requestMediator = requestMediator;
    }

    /// <summary>  </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public Task Consume(ConsumeContext<MassTransitCommand> context)
    {
        var commandRequestHandlerType = typeof(ICommandRequestHandler<>);
        const string method = nameof(ICommandRequestHandler<CommandRequest>.Handle);

        return !_requestMediator.HandlerImplemented(context.Message.Content, commandRequestHandlerType)
            ? Task.CompletedTask
            : _requestMediator.Execute(context.Message.Content, commandRequestHandlerType, method, context.CancellationToken);
    }
}
