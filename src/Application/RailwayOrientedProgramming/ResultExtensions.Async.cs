#if !NET40
using SharedKernel.Domain.RailwayOrientedProgramming;
using System.Linq;
using System.Threading.Tasks;

namespace SharedKernel.Application.RailwayOrientedProgramming;

/// <summary>  </summary>
public static partial class ResultExtensions
{
    /// <summary>  </summary>
    public static async Task<ApplicationResult<T>> ToApplicationResult<T>(this Task<Result<T>> resultTask)
    {
        var result = await resultTask;
        return result.IsSuccess
            ? ApplicationResult.Create(result.Value)
            : ApplicationResult.Failure<T>(result.Errors.Select(e =>
                ApplicationError.Create(e.ErrorMessage, e.PropertyName)));
    }

    /// <summary>  </summary>
    public static async Task<ApplicationResult<ApplicationUnit>> ToApplicationResultUnit<T>(this Task<Result<T>> resultTask)
    {
        var result = await resultTask;
        return result.IsSuccess
            ? ApplicationResult.Create(ApplicationUnit.Value)
            : ApplicationResult.Failure<ApplicationUnit>(result.Errors.Select(e =>
                ApplicationError.Create(e.ErrorMessage, e.PropertyName)));
    }
}

#endif