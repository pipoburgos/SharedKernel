using SharedKernel.Domain.Tests.Users;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Integration.Tests.Shared;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories
{
    public class EntityFrameworkCoreInMemoryRepositoryTests : InfrastructureTestCase
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SharedKernelDbContext>(options =>
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            return services;
        }

        [Fact]
        public void SaveRepositoryOk()
        {
            var repository = new UserEfCoreRepository(GetService<SharedKernelDbContext>());

            var roberto = User.Create(Guid.NewGuid(), "Roberto");
            repository.Add(roberto);

            repository.Save();

            Assert.Equal(roberto, repository.GetById(roberto.Id));
        }

        [Fact]
        public void SaveRepositoryNameChanged()
        {
            var repository = new UserEfCoreRepository(GetService<SharedKernelDbContext>());

            var roberto = User.Create(Guid.NewGuid(), "Roberto");
            repository.Add(roberto);

            repository.Save();

            var repoUser = repository.GetById(roberto.Id);
            repoUser.Name = "asdfass";

            Assert.Equal(roberto.Id, repoUser.Id);
            Assert.Equal(roberto.Name, repoUser.Name);
        }


    }
}
