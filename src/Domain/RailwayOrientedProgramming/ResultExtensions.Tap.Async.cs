// ReSharper disable InvokeAsExtensionMethod
#if !NET40

namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary>  </summary>
public static partial class ResultExtensions
{
    /// <summary>  </summary>
    public static async Task<Result<T>> Tap<T>(this Result<T> result, Func<T, Task> predicate)
    {
        if (result.IsSuccess)
            await predicate(result.Value);

        return result;
    }

    /// <summary>  </summary>
    public static async Task<Result<T>> Tap<T>(this Task<Result<T>> resultTask, Func<T, Task> predicate)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            await predicate(result.Value);

        return result;
    }

    /// <summary>  </summary>
    public static async Task<Result<T>> Tap<T>(this Task<Result<T>> resultTask, Action<T> predicate)
    {
        var result = await resultTask;
        if (result.IsSuccess)
            predicate(result.Value);

        return result;
    }

    /// <summary>  </summary>
    public static async Task<Result<T>> Tap<T, TU>(this Task<Result<T>> resultTask, Func<T, Task<Result<TU>>> predicate)
    {
        var result = await resultTask;
        if (!result.IsSuccess)
            return result;

        var resultPredicate = await predicate(result.Value);

        if (EqualityComparer<Result<TU>>.Default.Equals(default!, resultPredicate))
            return result;

        return resultPredicate.IsSuccess ? result : Result.Failure<T>(resultPredicate.Errors);
    }
}

#endif