namespace BankAccounts.Acceptance.Tests.Shared;

[CollectionDefinition("Factory")]
public class BankAccountClientFactoryCollection :
    //ICollectionFixture<DockerComposeCmdHook>,
    ICollectionFixture<BankAccountClientFactory>
{
}