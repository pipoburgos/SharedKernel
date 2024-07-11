using SharedKernel.Testing.Docker;

namespace BankAccounts.Acceptance.Tests.Shared;

[CollectionDefinition("Factory")]
public class BankAccountClientFactoryCollection :
    ICollectionFixture<DockerWslCmdHook>,
    ICollectionFixture<BankAccountClientFactory>
{
}