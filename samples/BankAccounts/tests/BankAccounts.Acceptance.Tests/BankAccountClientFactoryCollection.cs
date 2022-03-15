using Xunit;

namespace BankAccounts.Acceptance.Tests
{
    [CollectionDefinition("Factory")]
    public class BankAccountClientFactoryCollection : ICollectionFixture<BankAccountClientFactory>
    {
    }
}
