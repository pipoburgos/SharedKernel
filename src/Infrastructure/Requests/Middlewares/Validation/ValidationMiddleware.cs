using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Guards;

namespace SharedKernel.Infrastructure.Requests.Middlewares.Validation;

/// <summary>  </summary>
public class ValidationMiddleware : IMiddleware
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary> </summary>
    public ValidationMiddleware(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>  </summary>
    public async Task Handle<TRequest>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task> next) where TRequest : IRequest
    {
        var validationFailures = await ValidateAsync(request, cancellationToken);
        ThrowValidationFailureException(validationFailures);
        await next(request, cancellationToken);
    }

    /// <summary>  </summary>
    public async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken,
        Func<TRequest, CancellationToken, Task<TResponse>> next) where TRequest : IRequest<TResponse>
    {
        var validationFailures = await ValidateAsync(request, cancellationToken);
        ThrowValidationFailureException(validationFailures);
        return await next(request, cancellationToken);
    }

    /// <summary>  </summary>
    public async Task<ApplicationResult<TResponse>> Handle<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken, Func<TRequest, CancellationToken, Task<ApplicationResult<TResponse>>> next)
        where TRequest : IRequest<ApplicationResult<TResponse>>
    {
        var validationFailures = await ValidateAsync(request, cancellationToken);

        if (validationFailures.Any())
        {
            return ApplicationResult.Failure<TResponse>(
                validationFailures.Select(e => ApplicationError.Create(e.ErrorMessage, e.PropertyName)));
        }

        return await next(request, cancellationToken);
    }

    private static void ThrowValidationFailureException(List<ValidationFailure> validationFailures)
    {
        if (validationFailures.Any())
            throw new ValidationFailureException(validationFailures);
    }

    //private bool ValidatorIsRegistered<TRequest>(TRequest request) where TRequest : IRequest
    //{
    //    return _serviceProvider
    //        .CreateScope()
    //        .ServiceProvider
    //        .GetService(typeof(IValidator<>).MakeGenericType(request.GetType())) != default;
    //}

    //// ReSharper disable once UnusedMember.Local
    //private List<ValidationFailure> Validate<TRequest>(TRequest request)
    //{
    //    Guard.ThrowIfNull(request);
    //    var result = typeof(IEntityValidator<>).MakeGenericType(request!.GetType());
    //    var validator = _serviceProvider.CreateScope().ServiceProvider.GetService(result);

    //    if (validator == default)
    //        throw new Exception($"Validator '{result}' not found");

    //    const string methodName = nameof(IEntityValidator<object>.ValidateList);
    //    var method = validator.GetType().GetMethod(methodName);

    //    if (method == default)
    //        throw new Exception($"Method 'IValidator.{methodName}' not found");

    //    var failuresTaskObject = method.Invoke(validator, new object[] { request });

    //    if (failuresTaskObject is not List<ValidationFailure> validationResult)
    //        throw new Exception($"{nameof(List<ValidationFailure>)} null on executing '{result}.{methodName}'");

    //    return validationResult;
    //}

    // ReSharper disable once UnusedMember.Local
    private Task<List<ValidationFailure>> ValidateAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
    {
        Guard.ThrowIfNull(request);
        var result = typeof(IEntityValidator<>).MakeGenericType(request!.GetType());
        var validator = _serviceProvider.CreateScope().ServiceProvider.GetService(result);

        if (validator == default)
            throw new Exception($"Validator '{result}' not found");

        const string methodName = nameof(IEntityValidator<object>.ValidateListAsync);
        var method = validator.GetType().GetMethod(methodName);

        if (method == default)
            throw new Exception($"Method 'IValidator.{methodName}' not found");

        var failuresTaskObject = method.Invoke(validator, new object[] { request, cancellationToken });

        if (failuresTaskObject is not Task<List<ValidationFailure>> validationResult)
            throw new Exception($"{nameof(Task<List<ValidationFailure>>)} null on executing '{result}.{methodName}'");

        return validationResult;
    }
}
