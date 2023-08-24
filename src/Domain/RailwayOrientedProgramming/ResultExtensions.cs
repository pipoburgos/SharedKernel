namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary>  </summary>
// ReSharper disable once PartialTypeWithSinglePart
public static partial class ResultExtensions
{
    /// <summary>  </summary>
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if (result.IsFailure)
            return result;

        return predicate(result.Value) ? result : Result.Failure<T>(error);
    }

    /// <summary>  </summary>
    public static Result<T> EnsureAppendError<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if (predicate(result.Value))
            return result;

        var errors = result.Errors.ToList();
        errors.Add(error);
        return Result.Failure<T>(errors);
    }

    /// <summary>  </summary>
    public static Result<TU> Bind<T, TU>(this Result<T> result, Func<T, Result<TU>> predicate)
    {
        return result.IsSuccess
            ? predicate(result.Value)
            : Result.Failure<TU>(result.Errors);
    }

    /// <summary>  </summary>
    public static Result<T> Tap<T>(this Result<T> r, Action<T> predicate)
    {
        if (r.IsSuccess)
            predicate(r.Value);

        return r;
    }

    /// <summary>  </summary>
    public static Result<TU> Map<T, TU>(this Result<T> r, Func<T, TU> mapper)
    {
        return r.IsSuccess
            ? Result.Success(mapper(r.Value))
            : Result.Failure<TU>(r.Errors);
    }
}
