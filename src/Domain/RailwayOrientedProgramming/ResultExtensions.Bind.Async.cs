// ReSharper disable InvokeAsExtensionMethod
#if !NET40

namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary> . </summary>
public static partial class ResultExtensions
{
    /// <summary> . </summary>
    public static async Task<Result<TU>> Bind<T, TU>(this Task<Result<T>> resultTask, Func<T, Task<Result<TU>>> predicate)
    {
        var result = await resultTask;
        return result.IsSuccess
            ? await predicate(result.Value)
            : Result.Failure<TU>(result.Errors);
    }

    /// <summary> . </summary>
    public static async Task<Result<TU>> Bind<T, TU>(this Result<T> result, Func<T, Task<TU>> predicate)
    {
        return result.IsSuccess
            ? await predicate(result.Value)
            : Result.Failure<TU>(result.Errors);
    }

    /// <summary> . </summary>
    public static async Task<Result<TU>> TryBind<T, TU>(this Result<T> result, Func<T, Task<TU>> predicate,
        Func<Exception, Result<TU>>? capture = default, Func<Task>? finallyFunc = default)
    {
        if (!result.IsSuccess)
            return Result.Failure<TU>(result.Errors);

        Result<TU> resultError;
        try
        {
            return await predicate(result.Value);
        }
        catch (Exception ex)
        {
            resultError = capture?.Invoke(ex) ?? Result.Failure<TU>(Error.Create(ex.Message));
        }
        finally
        {
            await finallyFunc?.Invoke()!;
        }

        return resultError;
    }

    /// <summary> . </summary>
    public static async Task<Result<TU>> TryBind<T, TU, TException>(this Result<T> result, Func<T, Task<TU>> predicate,
        Func<TException, Result<TU>> captureCustom, Func<Exception, Result<TU>>? capture = default,
        Func<Task>? finallyFunc = default) where TException : Exception
    {
        if (!result.IsSuccess)
            return Result.Failure<TU>(result.Errors);

        Result<TU> resultError;
        try
        {
            return await predicate(result.Value);
        }
        catch (TException e)
        {
            resultError = captureCustom(e);
        }
        catch (Exception ex)
        {
            resultError = capture?.Invoke(ex) ?? Result.Failure<TU>(Error.Create(ex.Message));
        }
        finally
        {
            await finallyFunc?.Invoke()!;
        }

        return resultError;
    }
}

#endif