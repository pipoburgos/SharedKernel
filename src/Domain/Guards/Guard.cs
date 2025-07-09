namespace SharedKernel.Domain.Guards;

/// <summary>
/// Specifies that an output will not be null even if the corresponding type allows it.
/// Specifies that an input argument was not null when the call returns.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue)]
internal sealed class NotNullAttribute : Attribute
{
}


#if NET462_OR_GREATER || NETSTANDARD || NET6_0_OR_GREATER

#if !NET6_0_OR_GREATER

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
public static class Guard
{
    /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="throwOnNullEmptyOrWhiteSpaceString">Only applicable to strings.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    public static T ThrowIfNull<T>([NotNull] T argument, bool throwOnNullEmptyOrWhiteSpaceString = true,
        [CallerArgumentExpression(nameof(argument))] string? paramName = default)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(argument, paramName);
        if (throwOnNullEmptyOrWhiteSpaceString && argument is string s && string.IsNullOrWhiteSpace(s))
            throw new ArgumentNullException(paramName);
#else
        // ReSharper disable once ArrangeMissingParentheses
        if (argument is null ||
            throwOnNullEmptyOrWhiteSpaceString && argument is string s && string.IsNullOrWhiteSpace(s))
            throw new ArgumentNullException(paramName);
#endif
        return argument;
    }

    /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="throwOnNullEmptyOrWhiteSpaceString">Only applicable to strings.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    public static T ThrowIfNullOrDefault<T>([NotNull] T argument, bool throwOnNullEmptyOrWhiteSpaceString = true,
        [CallerArgumentExpression(nameof(argument))] string? paramName = default)
    {
        ThrowIfNull(argument, throwOnNullEmptyOrWhiteSpaceString, paramName);

        if (EqualityComparer<T>.Default.Equals(argument, default!))
            throw new ArgumentNullException(paramName);

        return argument;
    }
}
#endif
