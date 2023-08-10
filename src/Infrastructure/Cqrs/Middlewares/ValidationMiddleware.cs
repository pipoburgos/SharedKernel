using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Requests;
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
    /// <typeparam name="TRequest"></typeparam>
    public class ValidationMiddleware<TRequest> : IMiddleware<TRequest> where TRequest : IRequest
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        ///
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ValidationMiddleware(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
            var validatorIsRegistered = _serviceProvider.CreateScope().ServiceProvider.GetService(typeof(IValidator<>).MakeGenericType(request.GetType()));

            if (validatorIsRegistered == default)
                return next(request, cancellationToken);

            var result = typeof(IEntityValidator<>).MakeGenericType(request.GetType());
            var validator = _serviceProvider.CreateScope().ServiceProvider.GetService(result);

            if (validator == default)
                throw new Exception($"Validator '{result}'not found");

            return ValidateAsync(request, cancellationToken, next, validator);
        }

        private static async Task ValidateAsync(TRequest request, CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task> next, object validator)
        {
            const string methodName = "ValidateListAsync";
            var method = validator.GetType().GetMethod(methodName);

            if (method == default)
                throw new Exception($"Method '{methodName}'not found");

            var errors = method.Invoke(validator, new object[] { request, cancellationToken });

            var failures = await ((Task<List<ValidationFailure>>)errors)!;

            if (failures != default && failures.Any())
                throw new ValidationFailureException(failures);

            await next(request, cancellationToken);
        }
    }
}
