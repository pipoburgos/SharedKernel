using BankAccounts.Api;
using BankAccounts.Application;
using BankAccounts.Domain;
using BankAccounts.Infrastructure;
using SharedKernel.Testing.Architecture;
using System.Reflection;

namespace BankAccounts.Acceptance.Tests.ArquitectureTests;

public sealed class BankAccountsArchitectureTests : ArchitectureTests
{
    protected override Assembly GetDomainAssembly()
    {
        return typeof(BankAccountsDomainAssembly).Assembly;
    }

    protected override Assembly GetApplicationAssembly()
    {
        return typeof(BankAccountsApplicationAssembly).Assembly;
    }

    protected override Assembly GetInfrastructureAssembly()
    {
        return typeof(BankAccountsInfrastructureAssembly).Assembly;
    }

    protected override Assembly GetApiAssembly()
    {
        return typeof(BankAccountsApiAssembly).Assembly;
    }

    protected override Assembly GetAcceptanceTestsAssembly()
    {
        return typeof(BankAccountsArchitectureTests).Assembly;
    }

    protected override List<CheckFile> CheckFiles()
    {
        return [
            CheckFile.Handler,
            CheckFile.Endpoint,
            CheckFile.EndpointTests,
            CheckFile.Validator,
        ];
    }

    protected override bool CheckQueryValidators => true;

    protected override Assembly GetUseCasesTestsAssembly()
    {
        return typeof(BankAccountsArchitectureTests).Assembly;
    }

    protected override void Assert(IReadOnlyList<string>? failingTypeNames)
    {
        failingTypeNames.Should().BeNullOrEmpty();
    }
}
