using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Integration.Tests.Shared;
using System;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.InMemory
{
    public class EntityFrameworkCoreInMemoryRepositoryTests : InfrastructureTestCase
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<UserEfCoreRepository>();
            services.AddDbContext<SharedKernelDbContext>(options =>
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString()), ServiceLifetime.Singleton);

            return services;
        }

        [Fact]
        public void SaveRepositoryOk()
        {
            var roberto = User.Create(Guid.NewGuid(), "Roberto");
            GetService<UserEfCoreRepository>().Add(roberto);

            GetService<UserEfCoreRepository>().SaveChanges();

            Assert.Equal(roberto, GetService<UserEfCoreRepository>().GetById(roberto.Id));
        }

        [Fact]
        public void SaveRepositoryNameChanged()
        {
            var roberto = User.Create(Guid.NewGuid(), "Roberto");
            GetService<UserEfCoreRepository>().Add(roberto);

            GetService<UserEfCoreRepository>().SaveChanges();

            var repoUser = GetService<UserEfCoreRepository>().GetById(roberto.Id);
            repoUser.ChangeName("asdfass");

            Assert.Equal(roberto.Id, repoUser.Id);
            Assert.Equal(roberto.Name, repoUser.Name);
        }
    }
}
