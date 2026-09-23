using BankAccounts.Infrastructure;
using System.Reflection;

namespace BankAccounts.Acceptance.Tests.ArquitectureTests;

public class InfrastructureArchitectureTests : SharedKernel.Testing.Architecture.InfrastructureArchitectureTests
{
    protected override Assembly GetInfrastructureAssembly()
    {
        return typeof(BankAccountsInfrastructureAssembly).Assembly;
    }
}
