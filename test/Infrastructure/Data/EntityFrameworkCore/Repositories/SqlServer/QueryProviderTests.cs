using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Tests.Users;
#if NET461 || NETSTANDARD2_1
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;
#endif
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Infrastructure.Data.Queryable;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Extensions;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Queries;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.SqlServer;

[Collection("DockerHook")]
public class QueryProviderTests : IClassFixture<SqlServerApp>
{
    private readonly SqlServerApp _sqlServerApp;

    public QueryProviderTests(SqlServerApp sqlServerApp)
    {
        _sqlServerApp = sqlServerApp;
    }

    [Fact]
    public async Task SaveRepositoryNameChanged()
    {
        var queryProvider = _sqlServerApp.GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelEntityFrameworkDbContext>>();

        var tasks = new List<Task<List<User>>>();
        for (var i = 0; i < 22; i++)
        {
            tasks.Add(queryProvider.GetQuery<User>().Where(u => u.Name.Length != 23).ToListAsync());
        }

        var queries = await Task.WhenAll(tasks);
        queries.Length.Should().Be(22);
    }

    [Fact]
    public async Task ToPagedListFilterNullableInteger()
    {
        const int total = 3;

        var queryProvider = _sqlServerApp.GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelEntityFrameworkDbContext>>();

        var pageOptions = new PageOptions(default, default, default, true, false, new List<Order> { new("Name", true) },
            new List<FilterProperty> { new("NumberOfChildren", "220", FilterOperator.LessThanOrEqualTo) });

        var result = await queryProvider
            .GetQuery<User>()
            .ToPagedListAsync(pageOptions, CancellationToken.None);

        result.Items.Count().Should().Be(total * 2);
        result.TotalRecordsFiltered.Should().Be(total * 2);
    }

    [Fact]
    public async Task ToPagedListFilterNullableDateTime()
    {
        const int total = 3;

        var queryProvider = _sqlServerApp.GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelEntityFrameworkDbContext>>();

        var pageOptions = new PageOptions(default, default, default, true, false, new List<Order> { new("Name", true) },
            new List<FilterProperty> { new("Birthdate", "1970-05-01T00:00:00Z", FilterOperator.GreaterThan) });

        var result = await queryProvider
            .GetQuery<User>()
            .ToPagedListAsync(pageOptions, CancellationToken.None);

        result.Items.Count().Should().Be(total * 2);
        result.TotalRecordsFiltered.Should().Be(total * 2);
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


        var queryProvider = _sqlServerApp.GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelEntityFrameworkDbContext>>();

        var pageOptions = new PageOptions(0, 5, default, true, false, new List<Order> { new("Name", true) },
            new List<FilterProperty> { new("Name", "23", FilterOperator.NotStartsWith) });

        var result = await queryProvider
            .GetQuery<User>()
            .ToPagedListAsync(pageOptions, CancellationToken.None);

        result.Items.Count().Should().Be(1);
        result.TotalRecordsFiltered.Should().Be(1);
    }

    [Fact]
    public async Task ToPagedListContainsEmail()
    {


        var queryProvider = _sqlServerApp.GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelEntityFrameworkDbContext>>();

        var pageOptions = new PageOptions(0, 5, default, true, false, new List<Order> { new("Name", true) }
                , default);
        //,new List<FilterProperty> { new("emails", "a@a.es", FilterOperator.Contains) });

        //var result2 = await queryProvider
        //    .GetQuery<User>()
        //    .Where(u => u.Emails.Any(a => a == "a@a.es"))
        //    .ToListAsync();

        var result = await queryProvider
            .GetQuery<User>()
            //.Where(u => u.Emails.Contains("a@a.es"))
            .ToPagedListAsync(pageOptions, CancellationToken.None);

        result.Items.Count().Should().Be(5);
        result.TotalRecordsFiltered.Should().Be(6);
    }

    [Fact]
    public async Task ToPagedListParentName()
    {


        var queryProvider = _sqlServerApp.GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelEntityFrameworkDbContext>>();

        var pageOptions = new PageOptions(0, 5, default, true, false, new List<Order> { new("Name", true) },
            new List<FilterProperty> { new("parent.name", "abcde@a.es", FilterOperator.Contains) });

        var result = await queryProvider
            .GetQuery<User>()
            .ToPagedListAsync(pageOptions, CancellationToken.None);

        result.Items.Count().Should().BeGreaterOrEqualTo(0);
        result.TotalRecordsFiltered.Should().BeGreaterOrEqualTo(0);
    }

    [Fact]
    public async Task ToPagedListContainsLetterA()
    {


        var queryProvider = _sqlServerApp.GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelEntityFrameworkDbContext>>();

        var pageOptions = new PageOptions(0, 10, "aasdfa", true, false, new List<Order> { new("Name", true) },
            new List<FilterProperty>());

        var result = await queryProvider
            .GetQuery<User>()
            .ToPagedListAsync(pageOptions, CancellationToken.None);

        result.Items.Count().Should().BeGreaterOrEqualTo(0);
        result.TotalRecordsFiltered.Should().BeGreaterOrEqualTo(0);
    }

    [Fact]
    public async Task ToPagedListContainsCreatedToday()
    {


        var queryProvider = _sqlServerApp.GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelEntityFrameworkDbContext>>();

        var today = DateTime.Today;

        var pageOptions = new PageOptions(0, 5, default, true, false, new List<Order> { new("Name", true) },
            new List<FilterProperty> { new("CreatedAt", today.ToUniversalTime().ToString("O")) });

        var result = await queryProvider
            .GetQuery<User>()
            .ToPagedListAsync(pageOptions, CancellationToken.None);

        result.Items.Count().Should().Be(5);
        result.TotalRecordsFiltered.Should().Be(6);
    }

    [Fact]
    public async Task ToPagedListContainsCreatedNotToday()
    {


        var queryProvider = _sqlServerApp.GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelEntityFrameworkDbContext>>();

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


        const int total = 5;

        var queryProvider = _sqlServerApp.GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelEntityFrameworkDbContext>>();

        var pageOptions = new PageOptions(0, total, default, true, false, default,
            new List<FilterProperty> { new("NumberOfChildren", "150", FilterOperator.NotEqualTo) });

        var result = await queryProvider
            .GetQuery<User>()
            .Where(number.HasValue, x => x.NumberOfChildren != number!.Value)
            .Select(x => x.NumberOfChildren)
            .ToPagedListAsync(pageOptions, CancellationToken.None);

        result.Items.Count().Should().Be(total);

        using var dbContext = await _sqlServerApp.GetRequiredService<IDbContextFactory<SharedKernelEntityFrameworkDbContext>>().CreateDbContextAsync(CancellationToken.None);
        var repository = new EfCoreUserRepository(dbContext);
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


        const int total = 5;

        var queryProvider = _sqlServerApp.GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelEntityFrameworkDbContext>>();

        var pageOptions = new PageOptions(0, total, default, true, false, new List<Order> { new(default, false) },
            new List<FilterProperty> { new("NumberOfChildren", "150", FilterOperator.NotEqualTo) });

        var result = await queryProvider
            .GetQuery<User>()
            .Select(x => x.NumberOfChildren)
            .ToPagedListAsync(pageOptions, CancellationToken.None);

        result.Items.Count().Should().Be(total);

        using var dbContext = await _sqlServerApp.GetRequiredService<IDbContextFactory<SharedKernelEntityFrameworkDbContext>>().CreateDbContextAsync(CancellationToken.None);
        var repository = new EfCoreUserRepository(dbContext);
        var expected = (await repository.GetAllAsync(CancellationToken.None)).Select(x => x.NumberOfChildren).OrderByDescending(x => x).Take(total);
        result.Items.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task EntityFrameworkCoreQueryProviderJoin()
    {
        var cancellationToken = CancellationToken.None;
        await using var dbContext = await _sqlServerApp.GetRequiredService<IDbContextFactory<SharedKernelEntityFrameworkDbContext>>().CreateDbContextAsync(cancellationToken);
        await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        await dbContext.Database.MigrateAsync(cancellationToken);
        var user = UserMother.Create(Guid.NewGuid(), "a");
        await dbContext.Set<User>().AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var queryProvider = _sqlServerApp.GetRequiredService<EntityFrameworkCoreQueryProvider<SharedKernelEntityFrameworkDbContext>>();

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

