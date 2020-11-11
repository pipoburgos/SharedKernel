using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Infrastructure.Cqrs.Middlewares;

namespace SharedKernel.Infrastructure.Cqrs.Queries.InMemory
{
    public class InMemoryQueryBus : IQueryBus
    {
        private readonly ExecuteMiddlewaresService _executeMiddlewaresService;
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentDictionary<Type, object> QueryHandlers = new ConcurrentDictionary<Type, object>();

        public InMemoryQueryBus(
            ExecuteMiddlewaresService executeMiddlewaresService,
            IServiceProvider serviceProvider)
        {
            _executeMiddlewaresService = executeMiddlewaresService;
            _serviceProvider = serviceProvider;
        }

        public Task<TResponse> Ask<TResponse>(IQueryRequest<TResponse> query)
        {
            return Ask(query, CancellationToken.None);
        }

        public Task<TResponse> Ask<TResponse>(IQueryRequest<TResponse> query, CancellationToken cancellationToken)
        {
            _executeMiddlewaresService.Execute(query);

            var handler = GetWrappedHandlers(query);

            if (handler == null)
                throw new QueryNotRegisteredError(query.ToString());

            return handler.Handle(query, _serviceProvider, cancellationToken);
        }

        private QueryHandlerWrapper<TResponse> GetWrappedHandlers<TResponse>(IQueryRequest<TResponse> query)
        {
            Type[] typeArgs = { query.GetType(), typeof(TResponse) };

            var handlerType = typeof(IQueryRequestHandler<,>).MakeGenericType(typeArgs);
            var wrapperType = typeof(QueryHandlerWrapper<,>).MakeGenericType(typeArgs);

            var handlers =
                (IEnumerable)_serviceProvider.GetService(typeof(IEnumerable<>).MakeGenericType(handlerType));

            var wrappedHandlers = (QueryHandlerWrapper<TResponse>)QueryHandlers.GetOrAdd(query.GetType(), handlers.Cast<object>()
                .Select(handler => (QueryHandlerWrapper<TResponse>)Activator.CreateInstance(wrapperType)).FirstOrDefault());

            return wrappedHandlers;
        }
    }
}
