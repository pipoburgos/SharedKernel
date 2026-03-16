using TestResult = NetArchTest.Rules.TestResult;

namespace SharedKernel.Testing.Architecture;

public abstract class BaseArchitectureTest
{
    protected abstract void Assert(TestResult? testResult);
}
