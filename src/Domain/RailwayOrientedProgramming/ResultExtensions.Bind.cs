namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary> . </summary>
// ReSharper disable once PartialTypeWithSinglePart
public static partial class ResultExtensions
{
    /// <summary> . </summary>
    public static Result<TU> Bind<T, TU>(this Result<T> result, Func<T, Result<TU>> predicate)
    {
        return result.IsSuccess
            ? predicate(result.Value)
            : Result.Failure<TU>(result.Errors);
    }
    /// <summary> . </summary>
    public static Result<TU> TryBind<T, TU>(this Result<T> result, Func<T, TU> predicate,
        Func<Exception, Result<TU>>? capture = default, Action? finallyFunc = default)
    {
        if (!result.IsSuccess)
            return Result.Failure<TU>(result.Errors);

        Result<TU> resultError;
        try
        {
            return predicate(result.Value);
        }
        catch (Exception ex)
        {
            resultError = capture?.Invoke(ex) ?? Result.Failure<TU>(Error.Create(ex.Message));
        }
        finally
        {
            finallyFunc?.Invoke();
        }

        return resultError;
    }

    /// <summary> . </summary>
    public static Result<TU> TryBind<T, TU, TException>(this Result<T> result, Func<T, TU> predicate,
        Func<TException, Result<TU>> captureCustom, Func<Exception, Result<TU>>? capture = default,
        Action? finallyFunc = default) where TException : Exception
    {
        if (!result.IsSuccess)
            return Result.Failure<TU>(result.Errors);

        Result<TU> resultError;
        try
        {
            return predicate(result.Value);
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
            finallyFunc?.Invoke();
        }

        return resultError;
    }
}
