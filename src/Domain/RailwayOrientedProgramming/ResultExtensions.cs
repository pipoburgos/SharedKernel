#if !NET40
using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

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
    public static Result<TU> Bind<T, TU>(this Result<T> result, Func<T, Result<TU>> predicate)
    {
        try
        {
            return result.IsSuccess
                ? predicate(result.Value)
                : Result.Failure<TU>(result.Errors);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

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

    /// <summary>  </summary>
    public static Result<T> Then<T>(this Result<T> r, Action<T> predicate)
    {
        try
        {
            if (r.IsSuccess)
            {
                predicate(r.Value);
            }

            return r;
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>  </summary>
    public static Result<TU> Map<T, TU>(this Result<T> r, Func<T, TU> mapper)
    {
        try
        {
            return r.IsSuccess
                ? Result.Success(mapper(r.Value))
                : Result.Failure<TU>(r.Errors);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
}

#endif