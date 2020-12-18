using System;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Events;

namespace SharedKernel.Application.Cqrs.Middlewares
{
    /// <summary>
    /// Middleware that runs both on the command bus, as well as on the query and event bus
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public interface IMiddleware<TRequest> where TRequest : IRequest
    {
        /// <summary>
        /// Middleware that runs both on the command bus, as well as on the query and event bus
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        Task Handle(TRequest request, CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task> next);
    }

    /// <summary>
    /// Middleware that runs both on the command bus, as well as on the query and event bus
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IMiddleware<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Middleware that runs both on the command bus, as well as on the query and event bus
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task<TResponse>> next);
    }
}
