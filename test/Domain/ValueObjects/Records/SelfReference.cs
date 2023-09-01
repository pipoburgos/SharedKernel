namespace SharedKernel.Domain.Tests.ValueObjects.Records;

public record SelfReference
{
    public SelfReference(SelfReference? value)
    {
        Value = value;
    }

    public SelfReference? Value { get; private set; }
}