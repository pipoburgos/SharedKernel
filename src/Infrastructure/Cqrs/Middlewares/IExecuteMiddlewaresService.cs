using System;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Domain.Events;

namespace SharedKernel.Infrastructure.Cqrs.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExecuteMiddlewaresService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <param name="last"></param>
        /// <returns></returns>
        Task ExecuteAsync<TRequest>(TRequest request, CancellationToken cancellationToken,
            Func<TRequest, CancellationToken, Task> last) where TRequest : IRequest;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken,
            Func<TRequest, CancellationToken, Task<TResponse>> last) where TRequest : IRequest<TResponse>;
    }
}