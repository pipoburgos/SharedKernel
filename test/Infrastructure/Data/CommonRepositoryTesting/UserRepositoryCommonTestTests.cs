using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;

public abstract class UserRepositoryCommonTestTests<T> : InfrastructureTestCase<FakeStartup>
    where T : class, IRepositoryAsync<User, Guid>, ISaveRepository, ISaveRepositoryAsync
{
    [Fact]
    public void TestGetByIdAndSaveChanges()
    {
        BeforeStart();

        var repository = GetRequiredServiceOnNewScope<T>();
        var user = UserMother.Create();
        repository.Add(user);

        // Test not exists if not call SaveChanges
        var repository2 = GetRequiredServiceOnNewScope<T>();
        var userDdBb = repository2.GetById(user.Id);
        userDdBb.Should().BeNull();


        var total = repository.SaveChanges();

        total.Should().BeGreaterOrEqualTo(1);

        repository = GetRequiredServiceOnNewScope<T>();
        userDdBb = repository.GetById(user.Id);
        userDdBb.Should().NotBeNull();
        userDdBb!.Id.Should().Be(user.Id);
        userDdBb.CreatedAt.Should().BeAfter(DateTime.UtcNow.AddMinutes(-5));
    }

    [Fact]
    public void TestNameChangesTestUpdateMethod()
    {
        BeforeStart();

        var repository = GetRequiredServiceOnNewScope<T>();
        const string newName = "Test";
        var roberto = UserMother.Create();
        repository.Add(roberto);
        repository.SaveChanges();

        var repository2 = GetRequiredServiceOnNewScope<T>();
        var repoUser = repository2.GetById(roberto.Id);
        repoUser.Should().NotBeNull();
        repoUser!.ChangeName(newName);
        repository2.Update(repoUser);
        repository2.SaveChanges();

        var rob = GetRequiredServiceOnNewScope<T>().GetById(roberto.Id)!;
        rob.Name.Should().Be(newName);
        rob.LastModifiedAt.Should().BeAfter(DateTime.UtcNow.AddSeconds(-30));
    }

    [Fact]
    public void TestDeleteMethod()
    {
        BeforeStart();

        var repository = GetRequiredServiceOnNewScope<T>();
        var roberto = UserMother.Create();
        repository.Add(roberto);
        repository.SaveChanges();

        var repository2 = GetRequiredServiceOnNewScope<T>();
        var repoUser = repository2.GetById(roberto.Id);
        repoUser.Should().NotBeNull();
        repository2.Remove(repoUser!);
        repository2.SaveChanges();

        var rob = GetRequiredServiceOnNewScope<T>().GetById(roberto.Id)!;
        rob.Should().BeNull();
    }

    [Fact]
    public void TestPrivateListPropertiesAndValueObjects()
    {
        BeforeStart();

        var repository = GetRequiredServiceOnNewScope<T>();

        var roberto = UserMother.Create();

        for (var i = 0; i < 10; i++)
        {
            roberto.AddAddress(AddressMother.Create());
        }

        for (var i = 0; i < 5; i++)
        {
            roberto.AddEmail(EmailMother.Create());
        }

        repository.Add(roberto);
        repository.SaveChanges();

        var repository2 = GetRequiredServiceOnNewScope<T>();
        var repoUser = repository2.GetById(roberto.Id)!;

        repoUser.Should().NotBeNull();
        repoUser.Id.Should().Be(roberto.Id);
        repoUser.Name.Should().Be(roberto.Name);
        repoUser.NumberOfChildren.Should().Be(roberto.NumberOfChildren);
        repoUser.Emails.Count().Should().Be(5);
        repoUser.Addresses.Count().Should().Be(10);
        repoUser.Emails.Should().BeEquivalentTo(roberto.Emails);
        repoUser.CreatedAt.Should().BeAfter(DateTime.UtcNow.AddSeconds(-30));
    }

    [Fact]
    public async Task TestGetByIdAndSaveChangesAsync()
    {
        BeforeStart();

        var repository = GetRequiredServiceOnNewScope<T>();
        var user = UserMother.Create();
        await repository.AddAsync(user, CancellationToken.None);

        // Test not exists if not call SaveChanges
        var repository2 = GetRequiredServiceOnNewScope<T>();
        var userDdBb = await repository2.GetByIdAsync(user.Id, CancellationToken.None);
        userDdBb.Should().BeNull();


        var total = await repository.SaveChangesAsync(CancellationToken.None);

        total.Should().BeGreaterOrEqualTo(1);

        repository = GetRequiredServiceOnNewScope<T>();
        userDdBb = await repository.GetByIdAsync(user.Id, CancellationToken.None);
        userDdBb.Should().NotBeNull();
        userDdBb!.Id.Should().Be(user.Id);
        userDdBb.CreatedAt.Should().BeAfter(DateTime.UtcNow.AddMinutes(-5));
    }

    [Fact]
    public async Task TestNameChangesTestUpdateMethodAsync()
    {
        BeforeStart();

        var repository = GetRequiredServiceOnNewScope<T>();
        const string newName = "Test";
        var roberto = UserMother.Create();
        // Test synchronous and async methods together
        repository.Add(roberto);
        await repository.SaveChangesAsync(CancellationToken.None);

        var repository2 = GetRequiredServiceOnNewScope<T>();
        var repoUser = await repository2.GetByIdAsync(roberto.Id, CancellationToken.None);
        repoUser.Should().NotBeNull();
        repoUser!.ChangeName(newName);
        await repository2.UpdateAsync(repoUser, CancellationToken.None);
        await repository2.SaveChangesAsync(CancellationToken.None);

        var rob = await GetRequiredServiceOnNewScope<T>().GetByIdAsync(roberto.Id, CancellationToken.None);
        rob.Should().NotBeNull();
        rob!.Name.Should().Be(newName);
        rob.LastModifiedAt.Should().BeAfter(DateTime.UtcNow.AddMinutes(-5));
    }

    [Fact]
    public async Task TestDeleteMethodAsync()
    {
        BeforeStart();

        var repository = GetRequiredServiceOnNewScope<T>();
        var roberto = UserMother.Create();
        await repository.AddAsync(roberto, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        var repository2 = GetRequiredServiceOnNewScope<T>();
        var repoUser = await repository2.GetByIdAsync(roberto.Id, CancellationToken.None);
        repoUser.Should().NotBeNull();
        await repository2.RemoveAsync(repoUser!, CancellationToken.None);
        await repository2.SaveChangesAsync(CancellationToken.None);

        var rob = await GetRequiredServiceOnNewScope<T>().GetByIdAsync(roberto.Id, CancellationToken.None)!;
        rob.Should().BeNull();
    }

    [Fact]
    public async Task TestPrivateListPropertiesAndValueObjectsAsync()
    {
        BeforeStart();

        var repository = GetRequiredServiceOnNewScope<T>();

        var roberto = UserMother.Create();

        for (var i = 0; i < 10; i++)
        {
            roberto.AddAddress(AddressMother.Create());
        }

        for (var i = 0; i < 5; i++)
        {
            roberto.AddEmail(EmailMother.Create());
        }

        await repository.AddAsync(roberto, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        var repository2 = GetRequiredServiceOnNewScope<T>();
        var repoUser = await repository2.GetByIdAsync(roberto.Id, CancellationToken.None);

        repoUser.Should().NotBeNull();
        repoUser!.Id.Should().Be(roberto.Id);
        repoUser.Name.Should().Be(roberto.Name);
        repoUser.NumberOfChildren.Should().Be(roberto.NumberOfChildren);
        repoUser.Emails.Count().Should().Be(5);
        repoUser.Addresses.Count().Should().Be(10);
        repoUser.Emails.Should().BeEquivalentTo(roberto.Emails);
        repoUser.CreatedAt.Should().BeAfter(DateTime.UtcNow.AddMinutes(-5));
    }
}
