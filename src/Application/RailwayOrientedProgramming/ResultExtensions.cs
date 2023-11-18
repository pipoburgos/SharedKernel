using SharedKernel.Domain.RailwayOrientedProgramming;

namespace SharedKernel.Application.RailwayOrientedProgramming;

/// <summary>  </summary>
// ReSharper disable once PartialTypeWithSinglePart
public static partial class ResultExtensions
{
    /// <summary>  </summary>
    public static Result<T> ToApplicationResult<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? Result.Create(result.Value)
            : Result.Failure<T>(
                result.Errors.Select(e => ApplicationError.Create(e.ErrorMessage, e.PropertyName)));
    }

    /// <summary>  </summary>
    public static Result<ApplicationUnit> ToApplicationResultUnit<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? Result.Create(ApplicationUnit.Value)
            : Result.Failure<ApplicationUnit>(result.Errors.Select(e =>
                ApplicationError.Create(e.ErrorMessage, e.PropertyName)));
    }
}
