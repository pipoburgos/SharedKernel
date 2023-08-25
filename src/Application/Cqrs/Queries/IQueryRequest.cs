namespace SharedKernel.Application.Cqrs.Queries;

/// <summary> Query bus request abstaction. </summary>
// ReSharper disable once UnusedTypeParameter
public interface IQueryRequest<out TResponse> : IRequest<TResponse>
{
}
