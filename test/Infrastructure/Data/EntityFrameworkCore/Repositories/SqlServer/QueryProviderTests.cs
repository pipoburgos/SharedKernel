using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore;
#if NET461 || NETSTANDARD2_1 || NETCOREAPP3_1
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
#endif
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.Queries;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Integration.Tests.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.SqlServer
{
    [Collection("DockerHook")]
    public class QueryProviderTests : InfrastructureTestCase
    {
        protected override string GetJsonFile()
        {
            return "Data/EntityFrameworkCore/Repositories/SqlServer/appsettings.sqlServer.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddEntityFrameworkCoreSqlServer<SharedKernelDbContext>(Configuration, "SharedKernelSqlServer2");
        }


        [Fact]
        public async Task SaveRepositoryNameChanged()
        {
            await LoadTestDataAsync(CancellationToken.None);

            var queryProvider = GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelDbContext>>();

            var tasks = new List<Task<List<User>>>();
            for (var i = 0; i < 11; i++)
            {
                tasks.Add(queryProvider.GetQuery<User>().Where(u => u.Name.Length != 23).ToListAsync());
            }

            var queries = await Task.WhenAll(tasks);
            Assert.Equal(11, queries.Length);
        }

        private async Task LoadTestDataAsync(CancellationToken cancellationToken)
        {
            await using var dbContext = GetService<IDbContextFactory<SharedKernelDbContext>>().CreateDbContext();
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);

            var repository = new UserEfCoreRepository(dbContext);

            var tasks = new List<Task>();
            for (var i = 0; i < 11; i++)
            {
                var roberto = UserMother.Create();

                for (var j = 0; j < 10; j++)
                {
                    roberto.AddAddress(AddressMother.Create());
                }

                for (var j = 0; j < 5; j++)
                {
                    roberto.AddEmail(EmailMother.Create());
                }

                tasks.Add(repository.AddAsync(roberto, cancellationToken));
            }

            await Task.WhenAll(tasks);
            await repository.SaveChangesAsync(cancellationToken);
        }

        [Fact]
        public async Task EntityFrameworkCoreQueryProviderJoin()
        {
            var cancellationToken = CancellationToken.None;
            await using var dbContext = GetService<IDbContextFactory<SharedKernelDbContext>>().CreateDbContext();
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            var user = User.Create(Guid.NewGuid(), "a");
            await dbContext.Set<User>().AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var queryProvider = GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelDbContext>>();

            var query =
                from usuario1 in queryProvider.GetQuery<User>()
                join usuario2 in queryProvider.Set<User>() on usuario1.Id equals usuario2.Id
                select new
                {
                    NombresJuntos = usuario1.Name + usuario2.Name
                };

            var result = await query.FirstAsync(cancellationToken);

            Assert.Equal(result.NombresJuntos, user.Name + user.Name);
        }
    }
}
