namespace SharedKernel.Testing.Architecture;

public class BaseArchitectureTest
{
    protected virtual void Assert(IReadOnlyList<string>? failingTypeNames)
    {
        failingTypeNames?.Should().BeEmpty();
    }
}
