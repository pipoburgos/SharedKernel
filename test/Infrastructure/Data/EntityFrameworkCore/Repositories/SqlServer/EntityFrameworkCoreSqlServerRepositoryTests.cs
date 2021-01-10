﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infraestructure.Tests.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Infraestructure.Tests.Shared;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Infraestructure.Tests.Data.EntityFrameworkCore.Repositories.SqlServer
{
    public class EntityFrameworkCoreSqlServerRepositoryTests : InfrastructureTestCase
    {
        protected override string GetJsonFile()
        {
            return "Data/EntityFrameworkCore/Repositories/SqlServer/appsettings.sqlServer.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("SharedKernelSqlServer");
            return services
                .AddTransient<UserEfCoreRepository>()
                .AddDbContext<SharedKernelDbContext>(options => options.UseSqlServer(connection), ServiceLifetime.Transient);
        }

        [Fact]
        public async Task SaveRepositoryOk()
        {
            var dbContext = GetRequiredService<SharedKernelDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();

            var repository = GetRequiredService<UserEfCoreRepository>();

            var roberto = UserMother.Create();
            repository.Add(roberto);

            await repository.SaveChangesAsync(CancellationToken.None);

            Assert.Equal(roberto, repository.GetById(roberto.Id));

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();
        }

        [Fact]
        public async Task SaveRepositoryNameChanged()
        {
            var dbContext = GetService<SharedKernelDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();

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

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();
        }
    }
}
