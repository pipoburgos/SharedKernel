﻿using System;
#if NET6_0 || NET7_0
using System.Runtime.CompilerServices;
#endif

namespace SharedKernel.Domain.Guards;

/// <summary>
/// Specifies that an output will not be null even if the corresponding type allows it.
/// Specifies that an input argument was not null when the call returns.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue)]
internal sealed class NotNullAttribute : Attribute
{
}


#if !NET40 && !NET45 && !NET451 && !NET452 && !NET46 && !NET461

#if !NET5_0_OR_GREATER

[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class CallerArgumentExpressionAttribute : Attribute
{
    public CallerArgumentExpressionAttribute(string parameterName)
    {
        ParameterName = parameterName;
    }

    public string ParameterName { get; }
}

#endif

/// <summary> Throws an <see cref="ArgumentNullException"/> if is null. </summary>
public class Guard
{
    /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="throwOnEmptyString">Only applicable to strings.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    public static T ThrowIfNull<T>([NotNull] T argument, bool throwOnEmptyString = false,
        [CallerArgumentExpression("argument")] string paramName = null) where T : class
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(argument, paramName);
        if (throwOnEmptyString && argument is string s && string.IsNullOrEmpty(s))
            throw new ArgumentNullException(paramName);
#else
        // ReSharper disable once ArrangeMissingParentheses
        if (argument is null || throwOnEmptyString && argument is string s && string.IsNullOrEmpty(s))
            throw new ArgumentNullException(paramName);
#endif
        return argument;
    }
}
#endif