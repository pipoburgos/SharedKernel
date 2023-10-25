using SharedKernel.Application.UnitOfWorks;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;

public abstract class UserUnitOfWorkTests<T, TUnitOfWork> : InfrastructureTestCase<FakeStartup>
    where T : class, IRepositoryAsync<User, Guid> where TUnitOfWork : IUnitOfWorkAsync
{
    [Fact]
    public void TestUnitOfWorkSaveChanges()
    {
        BeforeStart();

        var unitOfWork = GetRequiredService<TUnitOfWork>();

        var unitOfWorkSame = GetRequiredService<TUnitOfWork>();

        unitOfWork.Id.Should().Be(unitOfWorkSame.Id);

        var unitOfWork2 = GetRequiredServiceOnNewScope<TUnitOfWork>();

        var repository = GetRequiredService<T>();
        var user = UserMother.Create();
        repository.Add(user);

        // Test not exists if not call SaveChanges
        var repository2 = GetRequiredService<T>();
        var userDdBb = repository2.GetById(user.Id);
        userDdBb.Should().BeNull();


        var total2 = unitOfWork2.SaveChanges();
        total2.Should().BeGreaterThanOrEqualTo(0);

        var total = unitOfWork.SaveChanges();
        total.Should().BeGreaterThanOrEqualTo(1);

        repository = GetRequiredService<T>();
        userDdBb = repository.GetById(user.Id);
        userDdBb.Should().NotBeNull();
        userDdBb!.Id.Should().Be(user.Id);
        userDdBb.CreatedAt.Should().BeAfter(DateTime.UtcNow.AddMinutes(-5));
    }

    [Fact]
    public void TestUnitOfWorkRollback()
    {
        BeforeStart();

        var unitOfWork = GetRequiredService<TUnitOfWork>();

        var repository = GetRequiredService<T>();
        var user = UserMother.Create();
        repository.Add(user);

        // Test not exists if not call SaveChanges
        var repository2 = GetRequiredService<T>();
        var userDdBb = repository2.GetById(user.Id);
        userDdBb.Should().BeNull();

        var total = unitOfWork.SaveChanges();
        total.Should().BeGreaterThanOrEqualTo(1);

        repository = GetRequiredService<T>();
        userDdBb = repository.GetById(user.Id);
        userDdBb.Should().NotBeNull();

        var total2 = unitOfWork.Rollback();
        total2.Should().BeGreaterThanOrEqualTo(1);

        repository = GetRequiredService<T>();
        userDdBb = repository.GetById(user.Id);
        userDdBb.Should().BeNull();
    }

    [Fact]
    public async Task TestUnitOfWorkRollbackAsync()
    {
        BeforeStart();

        var unitOfWork = GetRequiredService<TUnitOfWork>();

        var repository = GetRequiredService<T>();
        var user = UserMother.Create();
        await repository.AddAsync(user, CancellationToken.None);

        // Test not exists if not call SaveChanges
        var repository2 = GetRequiredService<T>();
        var userDdBb = await repository2.GetByIdAsync(user.Id, CancellationToken.None);
        userDdBb.Should().BeNull();

        var total = await unitOfWork.SaveChangesAsync(CancellationToken.None);
        total.Should().BeGreaterThanOrEqualTo(1);

        repository = GetRequiredService<T>();
        userDdBb = repository.GetById(user.Id);
        userDdBb.Should().NotBeNull();

        var total2 = await unitOfWork.RollbackAsync(CancellationToken.None);
        total2.Should().BeGreaterThanOrEqualTo(1);

        repository = GetRequiredService<T>();
        userDdBb = repository.GetById(user.Id);
        userDdBb.Should().BeNull();
    }
}
