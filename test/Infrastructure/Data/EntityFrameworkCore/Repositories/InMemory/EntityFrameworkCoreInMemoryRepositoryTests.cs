using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Testing.Infrastructure;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.InMemory
{
    public class EntityFrameworkCoreInMemoryRepositoryTests : InfrastructureTestCase<FakeStartup>
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
            var roberto = UserMother.Create(Guid.NewGuid(), "Roberto");
            GetRequiredService<UserEfCoreRepository>().Add(roberto);

            GetRequiredService<UserEfCoreRepository>().SaveChanges();

            Assert.Equal(roberto, GetRequiredService<UserEfCoreRepository>().GetById(roberto.Id));
        }

        [Fact]
        public void SaveRepositoryNameChanged()
        {
            var roberto = UserMother.Create(Guid.NewGuid(), "Roberto");
            GetRequiredService<UserEfCoreRepository>().Add(roberto);

            GetRequiredService<UserEfCoreRepository>().SaveChanges();

            var repoUser = GetRequiredService<UserEfCoreRepository>().GetById(roberto.Id);
            repoUser.ChangeName("asdfass");

            Assert.Equal(roberto.Id, repoUser.Id);
            Assert.Equal(roberto.Name, repoUser.Name);
        }
    }
}
