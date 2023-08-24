using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Infrastructure.Requests.Middlewares;
using System.Collections;
using System.Collections.Concurrent;

namespace SharedKernel.Infrastructure.Cqrs.Queries.InMemory
{
    /// <summary>
    /// 
    /// </summary>
    public class InMemoryQueryBus : IQueryBus
    {
        private readonly IExecuteMiddlewaresService _executeMiddlewaresService;
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentDictionary<Type, object> QueryHandlers = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executeMiddlewaresService"></param>
        /// <param name="serviceProvider"></param>
        public InMemoryQueryBus(
            IExecuteMiddlewaresService executeMiddlewaresService,
            IServiceProvider serviceProvider)
        {
            _executeMiddlewaresService = executeMiddlewaresService;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="query"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task<TResponse> Ask<TResponse>(IQueryRequest<TResponse> query, CancellationToken cancellationToken)
        {
            return _executeMiddlewaresService.ExecuteAsync(query, cancellationToken, (req, c) =>
            {
                var handler = GetWrappedHandlers(req);

                if (handler == null)
                    throw new QueryNotRegisteredException(req.ToString());

                return handler.Handle(req, _serviceProvider, c);
            });

        }

        private QueryHandlerWrapper<TResponse> GetWrappedHandlers<TResponse>(IQueryRequest<TResponse> query)
        {
            Type[] typeArgs = { query.GetType(), typeof(TResponse) };

            var handlerType = typeof(IQueryRequestHandler<,>).MakeGenericType(typeArgs);
            var wrapperType = typeof(QueryHandlerWrapper<,>).MakeGenericType(typeArgs);

            var handlers =
                (IEnumerable)_serviceProvider.GetRequiredService(typeof(IEnumerable<>).MakeGenericType(handlerType));

            var wrappedHandlers = (QueryHandlerWrapper<TResponse>)QueryHandlers.GetOrAdd(query.GetType(), handlers.Cast<object>()
                .Select(_ => (QueryHandlerWrapper<TResponse>)Activator.CreateInstance(wrapperType)).FirstOrDefault());

            return wrappedHandlers;
        }
    }
}
