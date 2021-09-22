using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Events;

namespace SharedKernel.Infrastructure.Cqrs.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public class TimerMiddleware<TRequest> : IMiddleware<TRequest> where TRequest : IRequest
    {
        private readonly ICustomLogger<TRequest> _logger;
        private readonly Stopwatch _timer;
        //private readonly IIdentityService _identityService;

        /// <summary>
        /// 
        /// </summary>
        public TimerMiddleware(ICustomLogger<TRequest> logger)//, IIdentityService identityService)
        {
            _logger = logger;
            _timer = new Stopwatch();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <param name="next"></param>
        /// <returns></returns>
        public Task Handle(TRequest request, CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task> next)
        {
            var name = typeof(TRequest).Name;

            _timer.Start();

            var response = next(request, cancellationToken);

            _timer.Stop();

            _logger.Verbose($"TimerBehaviour: {name} ({_timer.ElapsedMilliseconds} milliseconds) {request}");

            return response;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class TimerMiddleware<TRequest, TResponse> : IMiddleware<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ICustomLogger<TRequest> _logger;
        private readonly Stopwatch _timer;
        //private readonly IIdentityService _identityService;

        /// <summary>
        /// 
        /// </summary>
        public TimerMiddleware(ICustomLogger<TRequest> logger)//, IIdentityService identityService)
        {
            _logger = logger;
            _timer = new Stopwatch();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <param name="next"></param>
        /// <returns></returns>
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task<TResponse>> next)
        {
            var name = typeof(TRequest).Name;

            _timer.Start();

            var response = next(request, cancellationToken);

            _timer.Stop();

            _logger.Verbose($"TimerBehaviour: {name} ({_timer.ElapsedMilliseconds} milliseconds) {request}");
            //_logger.Info($"TimerBehaviour: {name} ({_timer.ElapsedMilliseconds} milliseconds) {_identityService.UserId} {request}");

            return response;
        }
    }
}
