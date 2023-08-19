using System;
using System.Linq;
#if !NET40
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
#endif

namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary>  </summary>
public static class ResultExtensions
{
    /// <summary>  </summary>
    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, string error)
    {
        if (result.IsFailure)
            return result;

        return predicate(result.Value) ? result : Result.Failure<T>(error);
    }

    /// <summary>  </summary>
    public static Result<T> EnsureAppendError<T>(this Result<T> result, Func<T, bool> predicate, string error)
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

#if !NET40
    /// <summary>  </summary>
    public static async Task<Result<TU>> Bind<T, TU>(this Task<Result<T>> result, Func<T, Task<Result<TU>>> predicate)
    {
        try
        {
            var r = await result;
            return r.IsSuccess
                ? await predicate(r.Value)
                : Result.Failure<TU>(r.Errors);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
#endif

    /// <summary>  </summary>
    public static Result<T> Then<T>(this Result<T> r, Action<T> predicate)
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

    /// <summary>  </summary>
    public static Result<TU> Merge<T, TU>(this Result<T> result, params Result<TU>[] results)
    {
        if (!result.IsFailure && results.All(r => !r.IsFailure))
            return results.Last();

        return Result.Failure<TU>(result.Errors.Concat(results.SelectMany(r => r.Errors)));
    }
}
