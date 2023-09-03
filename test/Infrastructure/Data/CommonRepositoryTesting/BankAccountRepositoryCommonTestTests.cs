using FluentAssertions;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Testing.Infrastructure;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;

public abstract class BankAccountRepositoryCommonTestTests<T> : InfrastructureTestCase<FakeStartup>
    where T : class, IRepository<BankAccount, Guid>, ISaveRepository
{
    protected virtual void Regenerate() { }

    [Fact]
    public void TestLogicalDeleteWithReadOneRepository()
    {
        Regenerate();

        var repository = GetRequiredServiceOnNewScope<T>();
        var bankAccount = BankAccountMother.Create().Value;
        repository.Add(bankAccount);

        // Test not exists if not call SaveChanges
        var repository2 = GetRequiredServiceOnNewScope<T>();
        var bankAccountDdBb = repository2.GetById(bankAccount.Id);
        bankAccountDdBb.Should().BeNull();


        var total = repository.SaveChanges();
        total.Should().BeGreaterOrEqualTo(1);

        repository = GetRequiredServiceOnNewScope<T>();
        bankAccountDdBb = repository.GetById(bankAccount.Id);
        bankAccountDdBb.Should().NotBeNull();
        repository.Remove(bankAccountDdBb!);
        var total2 = repository.SaveChanges();
        total2.Should().BeGreaterOrEqualTo(1);

        var repository3 = GetRequiredServiceOnNewScope<T>();
        var bankAccountDdBb3 = repository3.GetById(bankAccount.Id);
        bankAccountDdBb3.Should().BeNull();

        repository3.Any(bankAccount.Id).Should().BeFalse();

        repository3.NotAny(bankAccount.Id).Should().BeTrue();
    }

}
