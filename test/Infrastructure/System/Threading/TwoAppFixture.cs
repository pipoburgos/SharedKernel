using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.System.Threading;

public sealed class TwoAppFixture<TApp>
    where TApp : InfrastructureTestCase<FakeStartup>, new()
{
    public TApp App1 { get; } = new();
    public TApp App2 { get; } = new();
}