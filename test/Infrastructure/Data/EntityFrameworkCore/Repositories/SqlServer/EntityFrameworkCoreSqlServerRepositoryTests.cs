using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Testing.Infrastructure;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.SqlServer
{
    [Collection("DockerHook")]
    public class EntityFrameworkCoreSqlServerRepositoryTests : InfrastructureTestCase<FakeStartup>
    {
        protected override string GetJsonFile()
        {
            return "Data/EntityFrameworkCore/Repositories/SqlServer/appsettings.sqlServer.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("RepositoryConnectionString");
            return services
                .AddTransient<UserEfCoreRepository>()
                .AddDbContext<SharedKernelDbContext>(options => options.UseSqlServer(connection!), ServiceLifetime.Transient);
        }

        [Fact]
        public async Task SaveRepositoryOk()
        {
            await Regenerate();

            var repository = GetRequiredService<UserEfCoreRepository>();

            var roberto = UserMother.Create();
            repository.Add(roberto);

            await repository.SaveChangesAsync(CancellationToken.None);

            Assert.Equal(roberto, repository.GetById(roberto.Id));
        }

        [Fact]
        public async Task SaveRepositoryNameChanged()
        {
            await Regenerate();

            var repository = GetRequiredService<UserEfCoreRepository>();

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

            var repository2 = GetRequiredService<UserEfCoreRepository>();
            var repoUser = repository2.GetById(roberto.Id);

            Assert.Equal(roberto.Id, repoUser.Id);
            Assert.Equal(roberto.Name, repoUser.Name);
            Assert.Equal(5, repoUser.Emails.Count());
            Assert.Equal(10, repoUser.Addresses.Count());
        }

        private async Task Regenerate(CancellationToken cancellationToken = default)
        {
            var dbContext = GetRequiredService<SharedKernelDbContext>();
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
    }
}
