using SharedKernel.Domain.Repositories;

namespace SharedKernel.Domain.Tests.Users
{
    internal interface IUserRepository : ICreateRepository<User>
    {
    }
}
