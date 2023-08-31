namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary>  </summary>
// ReSharper disable once PartialTypeWithSinglePart
public static partial class ResultExtensions
{
    /// <summary>  </summary>
    public static Result<TU> Map<T, TU>(this Result<T> r, Func<T, TU> mapper)
    {
        return r.IsSuccess
            ? Result.Success(mapper(r.Value))
            : Result.Failure<TU>(r.Errors);
    }
}
