// ReSharper disable InvokeAsExtensionMethod
#if !NET40

namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary> . </summary>
public static partial class ResultExtensions
{
    /// <summary> . </summary>
    public static async Task<Result<TU>> Map<T, TU>(this Result<T> result, Func<T, Task<TU>> mapper)
    {
        return result.IsSuccess
            ? Result.Success(await mapper(result.Value))
            : Result.Failure<TU>(result.Errors);
    }

    /// <summary> . </summary>
    public static async Task<Result<TU>> Map<T, TU>(this Task<Result<T>> resultTask, Func<T, TU> mapper)
    {
        var result = await resultTask;
        return result.IsSuccess
            ? Result.Success(mapper(result.Value))
            : Result.Failure<TU>(result.Errors);
    }

    /// <summary> . </summary>
    public static async Task<Result<TU>> Map<T, TU>(this Task<Result<T>> resultTask, Func<T, Task<TU>> mapper)
    {
        var result = await resultTask;
        return result.IsSuccess
            ? Result.Success(await mapper(result.Value))
            : Result.Failure<TU>(result.Errors);
    }
}

#endif