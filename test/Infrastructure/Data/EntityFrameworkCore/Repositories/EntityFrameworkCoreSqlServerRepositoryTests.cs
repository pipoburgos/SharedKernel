using SharedKernel.Domain.Tests.Users;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Integration.Tests.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories
{
    public class EntityFrameworkCoreSqlServerRepositoryTests : InfrastructureTestCase
    {
        protected override string GetJsonFile()
        {
            return "Data/EntityFrameworkCore/Repositories/appsettings.sqlServer.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("SharedKernelSqlServer");
            return services.AddDbContext<SharedKernelDbContext>(options => options.UseSqlServer(connection), ServiceLifetime.Transient);
        }

        [Fact]
        public async Task SaveRepositoryOk()
        {
            var dbContext = GetService<SharedKernelDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();

            var repository = new UserEfCoreRepository(dbContext);

            var roberto = User.Create(Guid.NewGuid(), "Roberto bbdd");
            repository.Add(roberto);

            await repository.SaveChangesAsync(CancellationToken.None);

            Assert.Equal(roberto, repository.GetById(roberto.Id));
        }

        [Fact]
        public async Task SaveRepositoryNameChanged()
        {
            var dbContext = GetService<SharedKernelDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();

            var repository = new UserEfCoreRepository(dbContext);

            var roberto = User.Create(Guid.NewGuid(), "Roberto bbaa");
            repository.Add(roberto);

            repository.Save();

            var repository2 = new UserEfCoreRepository(GetService<SharedKernelDbContext>());
            var repoUser = repository2.GetById(roberto.Id);
            repoUser.Name = "asdfass";

            Assert.Equal(roberto.Id, repoUser.Id);
            Assert.NotEqual(roberto.Name, repoUser.Name);
        }
    }
}
