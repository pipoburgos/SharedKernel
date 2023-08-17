using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Application.Cqrs.Commands;

/// <summary> </summary>
public interface ICommandBusAsync
{
    /// <summary>
    /// Dispatch a command request that does not return anything
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task Dispatch(CommandRequest command, CancellationToken cancellationToken);

    /// <summary>
    /// Dispatch commands requests that does not return anything
    /// </summary>
    /// <param name="commands"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task Dispatch(IEnumerable<CommandRequest> commands, CancellationToken cancellationToken);
}
