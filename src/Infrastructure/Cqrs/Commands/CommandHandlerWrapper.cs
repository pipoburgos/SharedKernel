using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;

namespace SharedKernel.Infrastructure.Cqrs.Commands;

internal abstract class CommandHandlerWrapper
{
    public abstract Task Handle(ICommandRequest commandRequest, IServiceProvider provider,
        CancellationToken cancellationToken);
}

internal class CommandHandlerWrapper<TCommand> : CommandHandlerWrapper where TCommand : ICommandRequest
{
    public override Task Handle(ICommandRequest commandRequest, IServiceProvider provider, CancellationToken cancellationToken)
    {
        var handler = (ICommandRequestHandler<TCommand>) provider.CreateScope().ServiceProvider
            .GetRequiredService(typeof(ICommandRequestHandler<TCommand>));

        return handler.Handle((TCommand)commandRequest, cancellationToken);
    }
}

internal abstract class CommandHandlerWrapperResponse<TResponse>
{
    public abstract Task<TResponse> Handle(ICommandRequest<TResponse> commandRequest, IServiceProvider provider, CancellationToken cancellationToken);
}


internal class CommandHandlerWrapperResponse<TCommand, TResponse> : CommandHandlerWrapperResponse<TResponse> where TCommand: ICommandRequest<TResponse>
{
    public override Task<TResponse> Handle(ICommandRequest<TResponse> commandRequest, IServiceProvider provider, CancellationToken cancellationToken)
    {
        var handler = (ICommandRequestHandler<TCommand, TResponse>) provider.CreateScope().ServiceProvider
            .GetRequiredService(typeof(ICommandRequestHandler<TCommand, TResponse>));

        return handler.Handle((TCommand)commandRequest, cancellationToken);
    }
}