using SharedKernel.Domain.Requests;

namespace SharedKernel.Application.Cqrs.Commands.Handlers;

/// <summary> Command request that does not return anything. </summary>
public abstract class CommandRequest : Request, ICommandRequest
{
}
