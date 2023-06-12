using SharedKernel.Domain.ValueObjects;

namespace SharedKernel.Domain.Tests.ValueObjects.CustomClasses;

public class SelfReference : ValueObject<SelfReference>
{
    public SelfReference(SelfReference value)
    {
        Value = value;
    }

    public SelfReference Value { get; private set; }
}