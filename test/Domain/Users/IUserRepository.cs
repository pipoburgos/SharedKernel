using SharedKernel.Domain.Repositories.Create;

namespace SharedKernel.Domain.Tests.Users;

internal interface IUserRepository : ICreateRepository<User>
{
}