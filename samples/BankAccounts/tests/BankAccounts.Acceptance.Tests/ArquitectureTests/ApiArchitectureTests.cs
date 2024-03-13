using BankAccounts.Api;
using BankAccounts.Application;
using BankAccounts.Domain;
using BankAccounts.Infrastructure;
using NetArchTest.Rules;
using SharedKernel.Testing.Architecture;
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

    [Fact]
    public void TestCqrs()
    {
        new List<Assembly>
        {
            typeof(BankAccountsDomainAssembly).Assembly,
            typeof(BankAccountsApplicationAssembly).Assembly,
            typeof(BankAccountsInfrastructureAssembly).Assembly,
            typeof(BankAccountsApiAssembly).Assembly,
            typeof(ApiArchitectureTests).Assembly
        }.TestCqrsArquitecture([
            CheckFile.Handler,
            CheckFile.Endpoint
            //CheckFile.EndpointTests,
            //CheckFile.Validator,
            //CheckFile.HandlerTests
        ]);//, true);
    }
}
