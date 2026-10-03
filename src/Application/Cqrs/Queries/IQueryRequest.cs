namespace SharedKernel.Application.Cqrs.Queries;

/// <summary> Query request that does not return anything. </summary>
public interface IQueryRequest : IRequest;

/// <summary> Query bus request abstaction. </summary>
// ReSharper disable once UnusedTypeParameter
public interface IQueryRequest<out TResponse> : IRequest<TResponse>;
