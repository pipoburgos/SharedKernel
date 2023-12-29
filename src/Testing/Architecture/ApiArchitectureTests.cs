using SharedKernel.Api.Endpoints;
using System.Reflection;

namespace SharedKernel.Testing.Architecture;

public abstract class ApiArchitectureTests : BaseArchitectureTest
{
    protected abstract Assembly GetApiAssembly();

    [Fact]
    public void Endpoints_Should_BeSealed_And_HaveNameEndingWithEndpoint()
    {
        // Arrange and Act
        var result = Types.InAssembly(GetApiAssembly())
            .That()
            .Inherit(typeof(EndpointBase))
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .And()
            .HaveNameEndingWith("Endpoint")
            .GetResult();

        // Assert
        Assert(result);
    }
}
