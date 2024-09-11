using SharedKernel.Infrastructure.Requests.Middlewares.Failover;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Requests.Middlewares;

/// <summary> . </summary>
public class EntityFrameworkCoreRequestFailoverRepository<TContext> : IDisposable, IRequestFailoverRepository, IAsyncDisposable where TContext : DbContext
{
    private readonly TContext _context;

    /// <summary> . </summary>
    public EntityFrameworkCoreRequestFailoverRepository(IDbContextFactory<TContext> dbContextFactory)
    {
        _context = dbContextFactory.CreateDbContext();
    }

    /// <summary> . </summary>
    public async Task Save(ErrorRequest request, CancellationToken cancellationToken)
    {
        await _context.Set<ErrorRequest>().AddAsync(request, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary> . </summary>
    public void Dispose()
    {
        if (_context is IDisposable contextDisposable)
            contextDisposable.Dispose();
        else
            _ = _context.DisposeAsync().AsTask();
    }

    /// <summary> . </summary>
    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
