using BankAccounts.Api;
using NetArchTest.Rules;
using System.Reflection;

namespace BankAccounts.Acceptance.Tests.ArquitectureTests;

public class ApiArchitectureTests : SharedKernel.Testing.Architecture.ApiArchitectureTests
{
    protected override void Assert(TestResult? testResult)
    {
        testResult?.IsSuccessful.Should().BeTrue();
    }

    protected override Assembly GetApiAssembly()
    {
        return typeof(BankAccountsApiAssembly).Assembly;
    }
}
