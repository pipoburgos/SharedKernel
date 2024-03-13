using BankAccounts.Domain;
using SharedKernel.Testing.Architecture;

namespace BankAccounts.Acceptance.Tests.ArquitectureTests;

public class DomainArchitectureTests
{
    [Fact]
    public void DomainEventsShouldBeSealed()
    {
        var result = typeof(BankAccountsDomainAssembly).Assembly.TestDomainEventsShouldBeSealed();

        result.Should().BeEmpty();
    }

    [Fact]
    public void EntitiesShouldHavePublicFactory()
    {
        var result = typeof(BankAccountsDomainAssembly).Assembly.TestEntitiesShouldHavePublicFactory();

        result.Should().BeEmpty();
    }

    [Fact]
    public void EntitiesShouldNotHavePublicConstructors()
    {
        var result = typeof(BankAccountsDomainAssembly).Assembly.TestEntitiesShouldNotHavePublicConstructors();

        result.Should().BeEmpty();
    }
}
