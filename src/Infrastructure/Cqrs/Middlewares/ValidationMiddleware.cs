using System;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Validator;

namespace SharedKernel.Infrastructure.Cqrs.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IMiddleware<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEntityValidator<TRequest> _entityValidator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityValidator"></param>
        public ValidationBehavior(IEntityValidator<TRequest> entityValidator)
        {
            _entityValidator = entityValidator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task<TResponse>> next)
        {
            _entityValidator.Validate(request);

            return next(request, cancellationToken);
        }
    }
}
