using SharedKernel.Domain.RailwayOrientedProgramming;
using System;

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
            throw new Exception(string.Join(",", email.Errors));

        return email.Value;
    }

    /// <summary> </summary>
    public static Result<Email> CreateResult(string value)
    {
        return Result<Email>
            .Create(default)
            .EnsureAppendError(
                _ => string.IsNullOrWhiteSpace(value) || value.Contains("@"),
                "Invalid email")
            .Map(_ => new Email(value));
    }

    /// <summary> </summary>
    public string Value { get; private set; } = null!;

    /// <summary> </summary>
    public override string ToString()
    {
        return Value;
    }
}
