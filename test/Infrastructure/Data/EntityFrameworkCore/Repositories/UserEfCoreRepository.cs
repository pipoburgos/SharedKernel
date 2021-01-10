using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.Repositories;
using SharedKernel.Infraestructure.Tests.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Infraestructure.Tests.Data.EntityFrameworkCore.Repositories
{
    internal class UserEfCoreRepository : EntityFrameworkCoreRepositoryAsync<User>
    {
        public UserEfCoreRepository(SharedKernelDbContext sharedKernelDbContext) : base(sharedKernelDbContext)
        {
        }
    }
}
