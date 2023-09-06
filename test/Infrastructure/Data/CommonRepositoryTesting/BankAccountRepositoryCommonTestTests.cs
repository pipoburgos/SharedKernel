using FluentAssertions;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Testing.Infrastructure;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;

public abstract class BankAccountRepositoryCommonTestTests<T> : InfrastructureTestCase<FakeStartup>
    where T : class, IRepositoryAsync<BankAccount, Guid>, ISaveRepository, ISaveRepositoryAsync
{
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
