using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Queries;

namespace SharedKernel.Infrastructure.Cqrs.Queries;

internal abstract class QueryHandlerWrapper
{
    public abstract Task Handle(IQueryRequest query, IServiceProvider provider,
        CancellationToken cancellationToken);
}

internal class QueryHandlerWrapper<TQuery> : QueryHandlerWrapper where TQuery : IQueryRequest
{
    public override Task Handle(IQueryRequest query, IServiceProvider provider, CancellationToken cancellationToken)
    {
        var handler = (IQueryRequestHandler<TQuery>)provider.CreateScope().ServiceProvider
            .GetRequiredService(typeof(IQueryRequestHandler<TQuery>));

        return handler.Handle((TQuery)query, cancellationToken);
    }
}

internal abstract class QueryHandlerWrapperResponse<TResponse>
{
    public abstract Task<TResponse> Handle(IQueryRequest<TResponse> query, IServiceProvider provider,
        CancellationToken cancellationToken);
}

internal class QueryHandlerWrapperResponse<TQuery, TResponse> : QueryHandlerWrapperResponse<TResponse> where TQuery : IQueryRequest<TResponse>
{
    public override Task<TResponse> Handle(IQueryRequest<TResponse> query, IServiceProvider provider, CancellationToken cancellationToken)
    {
        var handler = (IQueryRequestHandler<TQuery, TResponse>)provider.CreateScope().ServiceProvider
            .GetRequiredService(typeof(IQueryRequestHandler<TQuery, TResponse>));

        return handler.Handle((TQuery)query, cancellationToken);
    }
}