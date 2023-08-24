using SharedKernel.Domain.ValueObjects;

namespace SharedKernel.Domain.Tests.ValueObjects.CustomClasses;

public class Integers : ValueObject<Integers>
{
    public Integers(List<int> ints)
    {
        Ints = ints;
    }

    public List<int> Ints { get; private set; }
}