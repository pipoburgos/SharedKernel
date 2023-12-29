using BankAccounts.Infrastructure;
using NetArchTest.Rules;
using System.Reflection;

namespace BankAccounts.Acceptance.Tests.ArquitectureTests;

public class InfrastructureArchitectureTests : SharedKernel.Testing.Architecture.InfrastructureArchitectureTests
{
    protected override Assembly GetInfrastructureAssembly()
    {
        return typeof(BankAccountsInfrastructureAssembly).Assembly;
    }

    protected override void Assert(TestResult? testResult)
    {
        testResult?.IsSuccessful.Should().BeTrue();
    }
}
