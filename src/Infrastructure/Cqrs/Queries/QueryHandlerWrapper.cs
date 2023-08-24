using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Queries;

namespace SharedKernel.Infrastructure.Cqrs.Queries
{
    internal abstract class QueryHandlerWrapper<TResponse>
    {
        public abstract Task<TResponse> Handle(IQueryRequest<TResponse> query, IServiceProvider provider,
            CancellationToken cancellationToken);
    }

    internal class QueryHandlerWrapper<TQuery, TResponse> : QueryHandlerWrapper<TResponse> where TQuery : IQueryRequest<TResponse>
    {
        public override Task<TResponse> Handle(IQueryRequest<TResponse> query, IServiceProvider provider, CancellationToken cancellationToken)
        {
            var handler = (IQueryRequestHandler<TQuery, TResponse>) provider.CreateScope().ServiceProvider
                .GetRequiredService(typeof(IQueryRequestHandler<TQuery, TResponse>));

            return handler.Handle((TQuery)query, cancellationToken);
        }
    }
}
