using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Tests.Users;
#if NET461 || NETSTANDARD2_1 || NETCOREAPP3_1
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
#endif
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Infrastructure.Data.Queryable;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Extensions;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Queries;
using SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer;
using SharedKernel.Testing.Infrastructure;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.SqlServer
{
    [Collection("DockerHook")]
    public class QueryProviderTests : InfrastructureTestCase<FakeStartup>
    {
        protected override string GetJsonFile()
        {
            return "Data/EntityFrameworkCore/Repositories/SqlServer/appsettings.sqlServer.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddEntityFrameworkCoreSqlServer<SharedKernelDbContext, ISharedKernelUnitOfWork>(Configuration.GetConnectionString("QueryProviderConnectionString")!);
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
            queries.Length.Should().Be(11);
        }

        [Fact]
        public async Task ToPagedListFilterNullableInteger()
        {
            const int total = 50;
            await LoadTestDataAsync(CancellationToken.None, total);

            var queryProvider = GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelDbContext>>();

            var pageOptions = new PageOptions(default, default, default, true, false, new List<Order> { new("Name", true) },
                new List<FilterProperty> { new("NumberOfChildren", "110", FilterOperator.LessThanOrEqualTo) });

            var result = await queryProvider
                .GetQuery<User>()
                .ToPagedListAsync(pageOptions, CancellationToken.None);

            result.Items.Count().Should().Be(total);
            result.TotalRecordsFiltered.Should().Be(total);
        }

        [Fact]
        public async Task ToPagedListFilterNullableDateTime()
        {
            const int total = 50;
            await LoadTestDataAsync(CancellationToken.None, total);

            var queryProvider = GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelDbContext>>();

            var pageOptions = new PageOptions(default, default, default, true, false, new List<Order> { new("Name", true) },
                new List<FilterProperty> { new("Birthdate", "1970-05-01T00:00:00Z", FilterOperator.GreaterThan) });

            var result = await queryProvider
                .GetQuery<User>()
                .ToPagedListAsync(pageOptions, CancellationToken.None);

            result.Items.Count().Should().Be(total);
            result.TotalRecordsFiltered.Should().Be(total);
        }
        //[Fact]
        //public async Task ToPagedListTwoQueries()
        //{
        //    const int total = 50;
        //    await LoadTestDataAsync(CancellationToken.None, total);

        //    var queryProvider = GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelDbContext>>();

        //    var pageOptions = new PageOptions(default, default, default, true, false, new List<Order> { new("Name", true) },
        //        new List<FilterProperty> { new("Name", "23", FilterOperator.NotStartsWith) });

        //    var result = await queryProvider
        //        .GetQuery<User, User>(pageOptions, 2)
        //       .ToPagedListAsync(pageOptions, CancellationToken.None);

        //    result.Items.Count().Should().Be(total);
        //    result.TotalRecordsFiltered.Should().Be(total);
        //}

        [Fact]
        public async Task ToPagedListNotStartsWith()
        {
            await LoadTestDataAsync(CancellationToken.None);

            var queryProvider = GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelDbContext>>();

            var pageOptions = new PageOptions(0, 10, default, true, false, new List<Order> { new("Name", true) },
                new List<FilterProperty> { new("Name", "23", FilterOperator.NotStartsWith) });

            var result = await queryProvider
                .GetQuery<User>()
               .ToPagedListAsync(pageOptions, CancellationToken.None);

            result.Items.Count().Should().Be(10);
            result.TotalRecordsFiltered.Should().Be(11);
        }

        [Fact]
        public async Task ToPagedListContainsLetterA()
        {
            await LoadTestDataAsync(CancellationToken.None);

            var queryProvider = GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelDbContext>>();

            var pageOptions = new PageOptions(0, 10, "a", true, false, new List<Order> { new("Name", true) },
                new List<FilterProperty>());

            var result = await queryProvider
                .GetQuery<User>()
               .ToPagedListAsync(pageOptions, CancellationToken.None);

            result.Items.Count().Should().BeGreaterThan(1);
            result.TotalRecordsFiltered.Should().BeGreaterThan(1);
        }

        [Fact]
        public async Task ToPagedListContainsCreatedToday()
        {
            await LoadTestDataAsync(CancellationToken.None);

            var queryProvider = GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelDbContext>>();

            var today = DateTime.Today;

            var pageOptions = new PageOptions(0, 10, default, true, false, new List<Order> { new("Name", true) },
                new List<FilterProperty> { new("CreatedAt", today.ToUniversalTime().ToString("O")) });

            var result = await queryProvider
                .GetQuery<User>()
                .ToPagedListAsync(pageOptions, CancellationToken.None);

            result.Items.Count().Should().Be(10);
            result.TotalRecordsFiltered.Should().Be(11);
        }

        [Fact]
        public async Task ToPagedListContainsCreatedNotToday()
        {
            await LoadTestDataAsync(CancellationToken.None);

            var queryProvider = GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelDbContext>>();

            var today = DateTime.Today;

            var pageOptions = new PageOptions(0, 10, default, true, false, new List<Order> { new("Name", true) },
                new List<FilterProperty>
                    {new("CreatedAt", today.ToUniversalTime().ToString("O"), FilterOperator.NotDateEqual)});

            var result = await queryProvider
                .GetQuery<User>()
               .ToPagedListAsync(pageOptions, CancellationToken.None);

            result.Items.Count().Should().Be(0);
            result.TotalRecordsFiltered.Should().Be(0);
        }

        [Theory]
        [InlineData(365)]
        [InlineData(0)]
        [InlineData(default)]
        public async Task ToPagedListOfPrimaryDto(int? number)
        {
            await LoadTestDataAsync(CancellationToken.None);

            const int total = 5;

            var queryProvider = GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelDbContext>>();

            var pageOptions = new PageOptions(0, total, default, true, false, default,
                new List<FilterProperty> { new("NumberOfChildren", "150", FilterOperator.NotEqualTo) });

            var result = await queryProvider
                .GetQuery<User>()
                .Where(number.HasValue, x => x.NumberOfChildren != number.Value)
                .Select(x => x.NumberOfChildren)
                .ToPagedListAsync(pageOptions, CancellationToken.None);

            result.Items.Count().Should().Be(total);

            using var dbContext = await GetService<IDbContextFactory<SharedKernelDbContext>>().CreateDbContextAsync(CancellationToken.None);
            var repository = new UserEfCoreRepository(dbContext);
            var expected = (await repository.GetAllAsync(CancellationToken.None))
                .Select(x => x.NumberOfChildren)
                .Where(x => x != number)
                .OrderBy(x => x)
                .Take(total);

            result.Items.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Fact]
        public async Task ToPagedListOfPrimaryDtoDescending()
        {
            await LoadTestDataAsync(CancellationToken.None);

            const int total = 5;

            var queryProvider = GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelDbContext>>();

            var pageOptions = new PageOptions(0, total, default, true, false, new List<Order> { new(default, false) },
                new List<FilterProperty> { new("NumberOfChildren", "150", FilterOperator.NotEqualTo) });

            var result = await queryProvider
                .GetQuery<User>()
                .Select(x => x.NumberOfChildren)
                .ToPagedListAsync(pageOptions, CancellationToken.None);

            result.Items.Count().Should().Be(total);

            using var dbContext = await GetService<IDbContextFactory<SharedKernelDbContext>>().CreateDbContextAsync(CancellationToken.None);
            var repository = new UserEfCoreRepository(dbContext);
            var expected = (await repository.GetAllAsync(CancellationToken.None)).Select(x => x.NumberOfChildren).OrderByDescending(x => x).Take(total);
            result.Items.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Fact]
        public async Task EntityFrameworkCoreQueryProviderJoin()
        {
            var cancellationToken = CancellationToken.None;
            await using var dbContext = GetService<IDbContextFactory<SharedKernelDbContext>>().CreateDbContext();
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            var user = UserMother.Create(Guid.NewGuid(), "a");
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

        private async Task LoadTestDataAsync(CancellationToken cancellationToken, int total = 11)
        {
            await using var dbContext = await GetService<IDbContextFactory<SharedKernelDbContext>>().CreateDbContextAsync(cancellationToken);
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);

            var repository = new UserEfCoreRepository(dbContext);

            var tasks = new List<Task>();
            for (var i = 0; i < total; i++)
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


    }
}
