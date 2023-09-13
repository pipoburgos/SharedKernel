using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Data.DbContexts;

namespace SharedKernel.Infrastructure.Data.UnitOfWorks;

/// <summary>  </summary>
public abstract class GlobalUnitOfWorkAsync : GlobalUnitOfWork, IGlobalUnitOfWorkAsync
{
    private readonly List<IDbContextAsync> _dbContextsAsync;

    private readonly List<IDbContextAsync> _dbContextsExecutedAsync;

    /// <summary>  </summary>
    protected GlobalUnitOfWorkAsync(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _dbContextsAsync = serviceProvider.GetServices<IDbContextAsync>().ToList();
        _dbContextsExecutedAsync = new List<IDbContextAsync>();
    }

    /// <summary>  </summary>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var total = 0;

        try
        {
            foreach (var dbContext in DbContexts.ToList())
            {
                total += dbContext.SaveChanges();
                DbContexts.Remove(dbContext);
                DbContextsExecuted.Add(dbContext);
            }

            foreach (var dbContextAsync in _dbContextsAsync.ToList())
            {
                total += await dbContextAsync.SaveChangesAsync(cancellationToken);
                _dbContextsAsync.Remove(dbContextAsync);
                _dbContextsExecutedAsync.Add(dbContextAsync);
            }
        }
        catch (Exception)
        {
            await RollbackAsync(cancellationToken);
        }

        return total;
    }

    /// <summary>  </summary>
    public async Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken)
    {
        var total = await SaveChangesAsync(cancellationToken);
        return Result.Create(total);
    }

    /// <summary>  </summary>
    public async Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        var total = 0;

        foreach (var dbContextAsync in DbContextsExecuted.ToList())
        {
            total += dbContextAsync.Rollback();
            DbContextsExecuted.Remove(dbContextAsync);
        }

        foreach (var dbContextAsync in _dbContextsExecutedAsync.ToList())
        {
            total += await dbContextAsync.RollbackAsync(cancellationToken);
            _dbContextsExecutedAsync.Remove(dbContextAsync);
        }

        DbContextsExecuted.Clear();
        _dbContextsExecutedAsync.Clear();

        return total;
    }

    /// <summary>  </summary>
    public async Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken)
    {
        var total = await RollbackAsync(cancellationToken);
        return Result.Create(total);
    }
}
