using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Logging;
using SharedKernel.Domain.Security;

namespace SharedKernel.Infrastructure.Cqrs.Middlewares
{
    public class TimerBehaviour<TRequest, TResponse> : IMiddleware<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ICustomLogger<TRequest> _customLogger;
        private readonly IIdentityService _identityService;

        public TimerBehaviour(ICustomLogger<TRequest> customLogger, IIdentityService identityService)
        {
            _timer = new Stopwatch();

            _customLogger = customLogger;
            _identityService = identityService;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task<TResponse>> next)
        {
            var name = typeof(TRequest).Name;

            _timer.Start();

            var response = next(request, cancellationToken);

            _timer.Stop();

            _customLogger.Info($"TimerBehaviour: {name} ({_timer.ElapsedMilliseconds} milliseconds) {_identityService.UserId} {request}");

            return response;
        }
    }
}
