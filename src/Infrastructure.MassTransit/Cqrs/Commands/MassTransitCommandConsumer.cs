using MassTransit;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.MassTransit.Cqrs.Commands;

/// <summary>  </summary>
public class MassTransitCommandConsumer : IConsumer<MassTransitCommand>
{
    private readonly IRequestMediator _requestMediator;

    /// <summary>  </summary>
    /// <param name="requestMediator"></param>
    public MassTransitCommandConsumer(IRequestMediator requestMediator)
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

        if (context.Message.Content == null ||
            !_requestMediator.HandlerImplemented(context.Message.Content, commandRequestHandlerType))
            return Task.CompletedTask;

        return _requestMediator.Execute(context.Message.Content, commandRequestHandlerType, method,
            context.CancellationToken);
    }
}
