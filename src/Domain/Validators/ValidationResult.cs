namespace SharedKernel.Domain.Validators;

/// <summary> </summary>
public class ValidationResult
{
    private readonly IEnumerable<string> _memberNames;

    /// <summary> </summary>
    /// <param name="errorMessage"></param>
    /// <param name="memberNames"></param>
    public ValidationResult(string errorMessage, IEnumerable<string>? memberNames = default)
    {
        ErrorMessage = errorMessage;
        // ReSharper disable once UseCollectionExpression
        _memberNames = memberNames ?? Enumerable.Empty<string>();
    }

    /// <summary> </summary>
    /// <param name="validationResult"></param>
    protected ValidationResult(ValidationResult validationResult)
    {
#if NET462_OR_GREATER || NETSTANDARD || NET6_0_OR_GREATER
        Guard.ThrowIfNull(validationResult);
#endif
        ErrorMessage = validationResult.ErrorMessage;
        _memberNames = validationResult._memberNames;
    }

    /// <summary> </summary>
    public IEnumerable<string> MemberNames => _memberNames;

    /// <summary> </summary>
    public string ErrorMessage { get; }

    /// <summary> </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return ErrorMessage;
    }
}
