using BankAccounts.Application;
using BankAccounts.Infrastructure;
using System.Reflection;
using TestResult = NetArchTest.Rules.TestResult;

namespace BankAccounts.Acceptance.Tests.ArquitectureTests;

public class ApplicationArchitectureTests : SharedKernel.Testing.Architecture.ApplicationArchitectureTests
{
    protected override Assembly GetApplicationAssembly()
    {
        return typeof(BankAccountsApplicationAssembly).Assembly;
    }

    protected override Assembly GetInfrastructureAssembly()
    {
        return typeof(BankAccountsInfrastructureAssembly).Assembly;
    }

    protected override void Assert(TestResult? testResult)
    {
        testResult?.IsSuccessful.Should().BeTrue();
    }
}
