using BankAccounts.Api;
using System.Reflection;

namespace BankAccounts.Acceptance.Tests.ArquitectureTests;

public class ApiArchitectureTests : SharedKernel.Testing.Architecture.ApiArchitectureTests
{
    protected override Assembly GetApiAssembly()
    {
        return typeof(BankAccountsApiAssembly).Assembly;
    }


}
