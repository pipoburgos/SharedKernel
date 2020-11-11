using System;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Events;

namespace SharedKernel.Application.Cqrs.Middlewares
{
    public interface IMiddleware<TRequest> where TRequest : IRequest
    {
        Task Handle(TRequest request, CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task> next);
    }

    public interface IMiddleware<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task<TResponse>> next);
    }
}
