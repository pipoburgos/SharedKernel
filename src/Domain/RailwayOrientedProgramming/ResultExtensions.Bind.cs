namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary>  </summary>
// ReSharper disable once PartialTypeWithSinglePart
public static partial class ResultExtensions
{
    /// <summary>  </summary>
    public static Result<TU> Bind<T, TU>(this Result<T> result, Func<T, Result<TU>> predicate)
    {
        return result.IsSuccess
            ? predicate(result.Value)
            : Result.Failure<TU>(result.Errors);
    }
}
