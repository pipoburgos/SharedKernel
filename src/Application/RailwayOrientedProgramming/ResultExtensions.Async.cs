#if !NET40
using SharedKernel.Domain.RailwayOrientedProgramming;

namespace SharedKernel.Application.RailwayOrientedProgramming;

/// <summary>  </summary>
public static partial class ResultExtensions
{
    /// <summary>  </summary>
    public static async Task<Result<T>> ToApplicationResult<T>(this Task<Result<T>> resultTask)
    {
        var result = await resultTask;
        return result.IsSuccess
            ? Result.Create(result.Value)
            : Result.Failure<T>(result.Errors.Select(e =>
                ApplicationError.Create(e.ErrorMessage, e.PropertyName)));
    }

    /// <summary>  </summary>
    public static async Task<Result<ApplicationUnit>> ToApplicationResultUnit<T>(this Task<Result<T>> resultTask)
    {
        var result = await resultTask;
        return result.IsSuccess
            ? Result.Create(ApplicationUnit.Value)
            : Result.Failure<ApplicationUnit>(result.Errors.Select(e =>
                ApplicationError.Create(e.ErrorMessage, e.PropertyName)));
    }
}

#endif