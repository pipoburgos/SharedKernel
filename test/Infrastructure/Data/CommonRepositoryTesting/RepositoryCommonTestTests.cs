using FluentAssertions;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Testing.Infrastructure;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;

public abstract class RepositoryCommonTestTests<T> : InfrastructureTestCase<FakeStartup>
    where T : class, IRepository<User, Guid>, ISaveRepository
{
    protected virtual void Regenerate() { }

    [Fact]
    public void TestGetByIdAndSaveChanges()
    {
        Regenerate();

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
    }

    [Fact]
    public void TestNameChangesTestUpdateMethod()
    {
        Regenerate();

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
    }

    [Fact]
    public void TestPrivateListPropertiesAndValueObjects()
    {
        Regenerate();

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
    }
}
