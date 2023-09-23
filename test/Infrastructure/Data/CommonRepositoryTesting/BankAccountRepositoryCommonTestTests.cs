using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;

public abstract class BankAccountRepositoryCommonTestTests<T> : InfrastructureTestCase<FakeStartup>
    where T : class, IRepositoryAsync<BankAccount, Guid>, ISaveRepositoryAsync
{
    [Fact]
    public void TestDistinctDbContextInstancesDbContextTransient()
    {
        BeforeStart();

        var repository = GetRequiredService<T>();
        var bankAccount = BankAccountMother.Create().Value;
        repository.Add(bankAccount);

        // Test not exists if not call SaveChanges
        var repository2 = GetRequiredService<T>();
        var bankAccountDdBb = repository2.GetById(bankAccount.Id);
        bankAccountDdBb.Should().BeNull();


        var total2 = repository2.SaveChanges();
        total2.Should().BeGreaterOrEqualTo(0);

        var total = repository.SaveChanges();
        total.Should().BeGreaterOrEqualTo(1);
    }

    [Fact]
    public void TestLogicalDeleteWithReadOneRepository()
    {
        BeforeStart();

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

    [Fact]
    public async Task TestLogicalDeleteWithReadOneRepositoryAsync()
    {
        BeforeStart();

        var repository = GetRequiredServiceOnNewScope<T>();
        var bankAccount = BankAccountMother.Create().Value;
        await repository.AddAsync(bankAccount, CancellationToken.None);

        // Test not exists if not call SaveChanges
        var repository2 = GetRequiredServiceOnNewScope<T>();
        var bankAccountDdBb = await repository2.GetByIdAsync(bankAccount.Id, CancellationToken.None);
        bankAccountDdBb.Should().BeNull();


        var total = await repository.SaveChangesAsync(CancellationToken.None);
        total.Should().BeGreaterOrEqualTo(1);

        repository = GetRequiredServiceOnNewScope<T>();
        bankAccountDdBb = await repository.GetByIdAsync(bankAccount.Id, CancellationToken.None);
        bankAccountDdBb.Should().NotBeNull();
        await repository.RemoveAsync(bankAccountDdBb!, CancellationToken.None);
        var total2 = await repository.SaveChangesAsync(CancellationToken.None);
        total2.Should().BeGreaterOrEqualTo(1);

        var repository3 = GetRequiredServiceOnNewScope<T>();
        var bankAccountDdBb3 = await repository3.GetByIdAsync(bankAccount.Id, CancellationToken.None);
        bankAccountDdBb3.Should().BeNull();

        repository3.Any(bankAccount.Id).Should().BeFalse();

        repository3.NotAny(bankAccount.Id).Should().BeTrue();
    }
}
