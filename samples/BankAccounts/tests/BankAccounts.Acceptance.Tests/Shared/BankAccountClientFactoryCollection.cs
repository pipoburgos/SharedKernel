using BankAccounts.Acceptance.Tests.Shared.Docker;

namespace BankAccounts.Acceptance.Tests.Shared
{
    [CollectionDefinition("Factory")]
    public class BankAccountClientFactoryCollection :
        ICollectionFixture<DockerHook>,
        ICollectionFixture<BankAccountClientFactory>
    {
    }
}
