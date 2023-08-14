#if !NET40
using SharedKernel.Domain.RailwayOrientedProgramming;
using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace SharedKernel.Application.RailwayOrientedProgramming;

/// <summary>  </summary>
public static class ResultExtensions
{
    /// <summary>  </summary>
    public static ApplicationResult<T> Ensure<T>(this ApplicationResult<T> result, Func<T, bool> predicate, string error)
    {
        if (result.IsFailure)
            return result;

        return predicate(result.Value) ? result : ApplicationResult.Failure<T>(error);
    }

    /// <summary>  </summary>
    public static ApplicationResult<T> EnsureAppendError<T>(this ApplicationResult<T> result, Func<T, bool> predicate, string error)
    {
        if (predicate(result.Value))
            return result;

        var errors = result.Errors.ToList();
        errors.Add(error);
        return ApplicationResult.Failure<T>(errors);
    }

    /// <summary>  </summary>
    public static ApplicationResult<TU> Bind<T, TU>(this ApplicationResult<T> result, Func<T, ApplicationResult<TU>> predicate)
    {
        try
        {
            return result.IsSuccess
                ? predicate(result.Value)
                : ApplicationResult.Failure<TU>(result.Errors);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>  </summary>
    public static async Task<ApplicationResult<TU>> Bind<T, TU>(this Task<ApplicationResult<T>> result, Func<T, Task<ApplicationResult<TU>>> predicate)
    {
        try
        {
            var r = await result;
            return r.IsSuccess
                ? await predicate(r.Value)
                : ApplicationResult.Failure<TU>(r.Errors);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>  </summary>
    public static ApplicationResult<T> Then<T>(this ApplicationResult<T> r, Action<T> predicate)
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
    public static ApplicationResult<TU> Map<T, TU>(this ApplicationResult<T> r, Func<T, TU> mapper)
    {
        try
        {
            return r.IsSuccess
                ? ApplicationResult.Success(mapper(r.Value))
                : ApplicationResult.Failure<TU>(r.Errors);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }

    /// <summary>  </summary>
    public static ApplicationResult<TU> Merge<T, TU>(this ApplicationResult<T> result, params ApplicationResult<TU>[] results)
    {
        if (!result.IsFailure && results.All(r => !r.IsFailure))
            return results.Last();

        return ApplicationResult.Failure<TU>(result.Errors.Concat(results.SelectMany(r => r.Errors)));
    }

    /// <summary>  </summary>
    public static ApplicationResult<T> CastToApplicationResult<T>(this Result<T> r)
    {
        try
        {
            return r.IsSuccess
                ? ApplicationResult.Create(r.Value)
                : ApplicationResult.Failure<T>(r.Errors);
        }
        catch (Exception e)
        {
            ExceptionDispatchInfo.Capture(e).Throw();
            throw;
        }
    }
}

#endif