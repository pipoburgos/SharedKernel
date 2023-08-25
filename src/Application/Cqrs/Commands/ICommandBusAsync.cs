namespace SharedKernel.Application.Cqrs.Commands;

/// <summary> </summary>
public interface ICommandBusAsync
{
    /// <summary> Dispatch a command request that does not return anything. </summary>
    Task Dispatch(CommandRequest command, CancellationToken cancellationToken);

    /// <summary> Dispatch commands requests that does not return anything. </summary>
    Task Dispatch(IEnumerable<CommandRequest> commands, CancellationToken cancellationToken);
}
