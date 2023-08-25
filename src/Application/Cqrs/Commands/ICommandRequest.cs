namespace SharedKernel.Application.Cqrs.Commands;

/// <summary> Command request that does not return anything. </summary>
public interface ICommandRequest : IRequest
{
}

/// <summary> Command request that returns a response. </summary>
public interface ICommandRequest<out TResponse> : IRequest<TResponse>
{
}
