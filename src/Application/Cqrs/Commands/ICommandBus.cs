namespace SharedKernel.Application.Cqrs.Commands;

/// <summary> Command bus. </summary>
public interface ICommandBus
{
    /// <summary> Dispatch a command that returns a response. </summary>
    Task<TResponse> Dispatch<TResponse>(ICommandRequest<TResponse> command, CancellationToken cancellationToken);

    /// <summary> Dispatch a command that returns a response. </summary>
    Task<ApplicationResult<TResponse>> Dispatch<TResponse>(ICommandRequest<ApplicationResult<TResponse>> command,
        CancellationToken cancellationToken);

    /// <summary> Dispatch a command request that does not return anything. </summary>
    Task Dispatch(ICommandRequest command, CancellationToken cancellationToken);

    /// <summary> Dispatch commands requests that does not return anything. </summary>
    Task Dispatch(IEnumerable<ICommandRequest> commands, CancellationToken cancellationToken);

    /// <summary> Dispatch commands requests that return something. </summary>
    Task<TResponse[]> Dispatch<TResponse>(IEnumerable<ICommandRequest<TResponse>> commands,
        CancellationToken cancellationToken);

    /// <summary> Dispatch commands requests that return something. </summary>
    Task<ApplicationResult<TResponse>[]> Dispatch<TResponse>(
        IEnumerable<ICommandRequest<ApplicationResult<TResponse>>> commands, CancellationToken cancellationToken);

    /// <summary> Dispatch commands requests on a queue. </summary>
    Task DispatchOnQueue(IEnumerable<ICommandRequest> commands, string queueName, CancellationToken cancellationToken);

    /// <summary> Dispatch a command request on a queue. </summary>
    Task DispatchOnQueue(ICommandRequest command, string queueName, CancellationToken cancellationToken);

    /// <summary> Dispatch a command request on a queue.. </summary>
    Task<TResponse> DispatchOnQueue<TResponse>(ICommandRequest<TResponse> command, string queueName,
        CancellationToken cancellationToken);

    /// <summary> Dispatch a command request on a queue.. </summary>
    Task<ApplicationResult<TResponse>> DispatchOnQueue<TResponse>(ICommandRequest<ApplicationResult<TResponse>> command,
        string queueName, CancellationToken cancellationToken);

    /// <summary> Queue a command request to background service. </summary>
    Task QueueInBackground(ICommandRequest command);
}
