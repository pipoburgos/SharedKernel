// ReSharper disable InvokeAsExtensionMethod
namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary>  </summary>
public static partial class ResultExtensions
{
    /// <summary>  </summary>
    public static Result<IEnumerable<T1>> Combine<T1>(this IEnumerable<Result<T1>> result)
    {
        var resultList = result.ToList();
        return resultList.All(r => r.IsSuccess)
            ? Result.Success(resultList.Select(r => r.Value))
            : Result.Failure<IEnumerable<T1>>(resultList.SelectMany(r => r.Errors));
    }

    /// <summary>  </summary>
    public static Result<Tuple<T1, T2>> Combine<T1, T2>(this Result<T1> result, Result<T2> result2)
    {
        if (!result.IsFailure && !result2.IsFailure)
            return Result.Success(new Tuple<T1, T2>(result.Value, result2.Value));

        return Result.Failure<Tuple<T1, T2>>(result.Errors.Concat(result2.Errors));
    }

    /// <summary>  </summary>
    public static Result<Tuple<T1, T2, T3>> Combine<T1, T2, T3>(this Result<T1> result, Result<T2> result2,
        Result<T3> result3)
    {
        if (!result.IsFailure && !result2.IsFailure && !result3.IsFailure)
            return Result.Success(new Tuple<T1, T2, T3>(result.Value, result2.Value, result3.Value));

        return Result.Failure<Tuple<T1, T2, T3>>(result.Errors.Concat(result2.Errors).Concat(result3.Errors));
    }

    /// <summary>  </summary>
    public static Result<Tuple<T1, T2, T3, T4>> Combine<T1, T2, T3, T4>(this Result<T1> result, Result<T2> result2,
        Result<T3> result3, Result<T4> result4)
    {
        if (!result.IsFailure && !result2.IsFailure && !result3.IsFailure && !result4.IsFailure)
            return Result.Success(new Tuple<T1, T2, T3, T4>(result.Value, result2.Value, result3.Value, result4.Value));

        return Result.Failure<Tuple<T1, T2, T3, T4>>(result.Errors.Concat(result2.Errors).Concat(result3.Errors)
            .Concat(result4.Errors));
    }

    /// <summary>  </summary>
    public static Result<Tuple<T1, T2, T3, T4, T5>> Combine<T1, T2, T3, T4, T5>(this Result<T1> result,
        Result<T2> result2, Result<T3> result3, Result<T4> result4, Result<T5> result5)
    {
        if (!result.IsFailure && !result2.IsFailure && !result3.IsFailure && !result4.IsFailure && !result5.IsFailure)
            return Result.Success(new Tuple<T1, T2, T3, T4, T5>(result.Value, result2.Value, result3.Value,
                result4.Value, result5.Value));

        return Result.Failure<Tuple<T1, T2, T3, T4, T5>>(result.Errors.Concat(result2.Errors).Concat(result3.Errors)
            .Concat(result4.Errors).Concat(result5.Errors));
    }
}
