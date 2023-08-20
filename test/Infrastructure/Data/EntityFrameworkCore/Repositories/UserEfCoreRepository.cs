using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories
{
    internal class UserEfCoreRepository : EntityFrameworkCoreRepositoryAsync<User>
    {
        public UserEfCoreRepository(SharedKernelDbContext sharedKernelDbContext) : base(sharedKernelDbContext)
        {
        }
    }
}
