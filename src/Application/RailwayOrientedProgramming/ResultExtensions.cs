using SharedKernel.Domain.RailwayOrientedProgramming;

namespace SharedKernel.Application.RailwayOrientedProgramming;

/// <summary>  </summary>
// ReSharper disable once PartialTypeWithSinglePart
public static partial class ResultExtensions
{
    /// <summary>  </summary>
    public static ApplicationResult<T> ToApplicationResult<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? ApplicationResult.Create(result.Value)
            : ApplicationResult.Failure<T>(
                result.Errors.Select(e => ApplicationError.Create(e.ErrorMessage, e.PropertyName)));
    }

    /// <summary>  </summary>
    public static ApplicationResult<ApplicationUnit> ToApplicationResultUnit<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? ApplicationResult.Create(ApplicationUnit.Value)
            : ApplicationResult.Failure<ApplicationUnit>(result.Errors.Select(e =>
                ApplicationError.Create(e.ErrorMessage, e.PropertyName)));
    }
}
