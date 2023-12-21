using SharedKernel.Api.Endpoints;
using System.Reflection;

namespace SharedKernel.Testing.Architecture;

public abstract class ApiArchitectureTests
{
    protected abstract Assembly GetApiAssembly();

    [Fact]
    public void Endpoints_Should_BeSealed()
    {
        // Arrange and Act
        var result = Types.InAssembly(GetApiAssembly())
            .That()
            .Inherit(typeof(EndpointBase))
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}
