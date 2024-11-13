using MassTransit;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.MassTransit.Cqrs.Commands;
using SharedKernel.Infrastructure.Requests;

namespace SharedKernel.Infrastructure.MassTransit.Events;

/// <summary>  </summary>
public class MassTransitEventConsumer : IConsumer<MassTransitCommand>
{
    private readonly IRequestMediator _requestMediator;

    /// <summary>  </summary>
    /// <param name="requestMediator"></param>
    public MassTransitEventConsumer(IRequestMediator requestMediator)
    {
        _requestMediator = requestMediator;
    }

    /// <summary>  </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public Task Consume(ConsumeContext<MassTransitCommand> context)
    {
        var commandRequestHandlerType = typeof(IDomainEventSubscriber<>);
        const string method = nameof(IDomainEventSubscriber<DomainEvent>.On);

        if (context.Message.Content == null ||
            !_requestMediator.HandlerImplemented(context.Message.Content, commandRequestHandlerType))
            return Task.CompletedTask;

        return _requestMediator.Execute(context.Message.Content, commandRequestHandlerType, method,
            context.CancellationToken);
    }
}
