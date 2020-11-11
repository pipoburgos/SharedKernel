using System;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Validator;

namespace SharedKernel.Infrastructure.Cqrs.Middlewares
{
    public class ValidationBehavior<TRequest, TResponse> : IMiddleware<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEntityValidator<TRequest> _entityValidator;

        public ValidationBehavior(IEntityValidator<TRequest> entityValidator)
        {
            _entityValidator = entityValidator;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task<TResponse>> next)
        {
            _entityValidator.Validate(request);

            return next(request, cancellationToken);
        }
    }
}
