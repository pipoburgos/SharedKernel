namespace SharedKernel.Application.Cqrs.Commands.Handlers;

/// <summary>
/// Handler of a command that does not return anything
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandRequestHandler<in TCommand> where TCommand : ICommandRequest
{
    /// <summary>
    /// The implementation of the command
    /// </summary>
    /// <param name="command">Command</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task Handle(TCommand command, CancellationToken cancellationToken);
}

/// <summary>
/// Handler of a command that returns a response
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface ICommandRequestHandler<in TCommand, TResponse> where TCommand : ICommandRequest<TResponse>
{
    /// <summary>
    /// The implementation of the command
    /// </summary>
    /// <param name="command">Command</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
}