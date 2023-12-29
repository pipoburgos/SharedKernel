using BankAccounts.Domain;
using NetArchTest.Rules;
using System.Reflection;

namespace BankAccounts.Acceptance.Tests.ArquitectureTests;

public class DomainArchitectureTests : SharedKernel.Testing.Architecture.DomainArchitectureTests
{
    protected override Assembly GetDomainAssembly()
    {
        return typeof(BankAccountsDomainAssembly).Assembly;
    }

    protected override void Assert(IEnumerable<Type>? failingTypes)
    {
        failingTypes?.Should().BeEmpty();
    }

    protected override void Assert(TestResult? testResult)
    {
        testResult?.IsSuccessful.Should().BeTrue();
    }
}
