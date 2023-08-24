#if !NET40

namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary>  </summary>
public static partial class ResultExtensions
{
    /// <summary>  </summary>
    public static async Task<Result<TU>> Bind<T, TU>(this Task<Result<T>> resultTask, Func<T, Task<Result<TU>>> predicate)
    {
        var result = await resultTask;
        return result.IsSuccess
            ? await predicate(result.Value)
            : Result.Failure<TU>(result.Errors);
    }

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
    public static async Task<Result<TU>> Map<T, TU>(this Task<Result<T>> resultTask, Func<T, TU> mapper)
    {
        var result = await resultTask;
        return result.IsSuccess
            ? Result.Success(mapper(result.Value))
            : Result.Failure<TU>(result.Errors);
    }
}

#endif