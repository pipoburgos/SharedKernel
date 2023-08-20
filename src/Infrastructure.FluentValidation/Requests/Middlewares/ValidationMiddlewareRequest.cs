using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Requests;
using SharedKernel.Application.Validator;

namespace SharedKernel.Infrastructure.FluentValidation.Requests.Middlewares;

/// <summary>  </summary>
public class ValidationMiddleware<TRequest, TResponse> : IMiddleware<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary> </summary>
    public ValidationMiddleware(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>  </summary>
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task<TResponse>> next)
    {
        var validatorIsRegistered = _serviceProvider.CreateScope().ServiceProvider
            .GetService(typeof(IValidator<>).MakeGenericType(request.GetType()));

        if (validatorIsRegistered == default)
            return next(request, cancellationToken);

        var result = typeof(IEntityValidator<>).MakeGenericType(request.GetType());
        var validator = _serviceProvider.CreateScope().ServiceProvider.GetService(result);

        if (validator == default)
            throw new Exception($"Validator '{result}'not found");

        return ValidateAsync(request, cancellationToken, next, validator);
    }

    private async Task<TResponse> ValidateAsync(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task<TResponse>> next, object validator)

    {
        const string methodName = "ValidateListAsync";
        var method = validator.GetType().GetMethod(methodName);

        if (method == default)
            throw new Exception($"Method '{methodName}'not found");

        var errors = method.Invoke(validator, new object[] { request, cancellationToken });

        var failures = await (errors as Task<List<ValidationFailure>>)!;

        if (failures != default && failures.Any())
            throw new ValidationFailureException(failures);

        return await next(request, cancellationToken);
    }
}