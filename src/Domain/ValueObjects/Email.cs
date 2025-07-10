using SharedKernel.Domain.Exceptions;

namespace SharedKernel.Domain.ValueObjects;

/// <summary> </summary>
public class Email : ValueObject<Email>
{
    /// <summary> </summary>
    protected Email() { }

    /// <summary> </summary>
    protected Email(string value) : this()
    {
        Value = value;
    }

    /// <summary> </summary>
    public static Email Create(string value)
    {
        var email = CreateResult(value);
        if (email.IsFailure)
            throw new TextException(string.Join(",", email.Errors));

        return email.Value;
    }

    /// <summary> </summary>
    public static Result<Email> CreateResult(string value) =>
        Result
            .Create(value)
            .EnsureAppendError(
                v => string.IsNullOrWhiteSpace(v) || v.Contains("@"),
                Error.Create("Invalid email", nameof(Email)))
            .Map(_ => new Email(value));

    /// <summary> </summary>
    public string Value { get; private set; } = null!;

    /// <summary> </summary>
    public override string ToString()
    {
        return Value;
    }
}
