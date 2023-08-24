#if NET47_OR_GREATER || NET5_0_OR_GREATER || NETSTANDARD

using System.Linq;

namespace SharedKernel.Domain.RailwayOrientedProgramming;

/// <summary>  </summary>
public static partial class ResultExtensions
{
    /// <summary>  </summary>
    public static Result<(T1, T2)> Combine<T1, T2>(this Result<T1> result, Result<T2> result2)
    {
        if (!result.IsFailure && !result2.IsFailure)
            return (result.Value, result2.Value).Success();

        return Result.Failure<(T1, T2)>(result.Errors.Concat(result2.Errors));
    }

    /// <summary>  </summary>
    public static Result<(T1, T2, T3)> Combine<T1, T2, T3>(this Result<T1> result, Result<T2> result2, Result<T3> result3)
    {
        if (!result.IsFailure && !result2.IsFailure && !result3.IsFailure)
            return (result.Value, result2.Value, result3.Value).Success();

        return Result.Failure<(T1, T2, T3)>(result.Errors.Concat(result2.Errors).Concat(result3.Errors));
    }
}

#endif
