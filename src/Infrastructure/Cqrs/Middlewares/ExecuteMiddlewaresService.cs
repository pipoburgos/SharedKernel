using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Cqrs.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class ExecuteMiddlewaresService : IExecuteMiddlewaresService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        public ExecuteMiddlewaresService(
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <param name="last"></param>
        /// <returns></returns>
        public Task ExecuteAsync<TRequest>(TRequest request, CancellationToken cancellationToken,
            Func<TRequest, CancellationToken, Task> last) where TRequest : IRequest
        {
            var middlewares = _serviceScopeFactory.CreateScope().ServiceProvider.GetServices<IMiddleware<TRequest>>().ToList();
            return !middlewares.Any()
                ? last(request, cancellationToken)
                : middlewares[0].Handle(request, cancellationToken, GetNext(middlewares, 1, middlewares.Count, last));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken,
            Func<TRequest, CancellationToken, Task<TResponse>> last) where TRequest : IRequest<TResponse>
        {
            var middlewares = _serviceScopeFactory.CreateScope().ServiceProvider.GetServices<IMiddleware<TRequest, TResponse>>().ToList();
            return !middlewares.Any()
                ? last(request, cancellationToken)
                : middlewares[0].Handle(request, cancellationToken, GetNext(middlewares, 1, middlewares.Count, last));
        }

        private Func<TRequest, CancellationToken, Task> GetNext<TRequest>(IList<IMiddleware<TRequest>> middlewares,
            int i, int lastIndex, Func<TRequest, CancellationToken, Task> last) where TRequest : IRequest
        {
            if (i == lastIndex)
                return last;

            i++;
            return (r, c) => middlewares[i - 1].Handle(r, c, GetNext(middlewares, i, lastIndex, last));
        }


        private Func<TRequest, CancellationToken, Task<TResponse>> GetNext<TRequest, TResponse>(IList<IMiddleware<TRequest, TResponse>> middlewares,
            int i, int lastIndex, Func<TRequest, CancellationToken, Task<TResponse>> last) where TRequest : IRequest<TResponse>
        {
            if (i == lastIndex)
                return last;

            i++;
            return (r, c) => middlewares[i - 1].Handle(r, c, GetNext(middlewares, i, lastIndex, last));
        }
    }
}
