using SharedKernel.Application.Communication.Email;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Communication.Email;

/// <summary>  </summary>
public class EntityFrameworkCoreOutboxMailRepository<TContext> : IDisposable, IOutboxMailRepository, IAsyncDisposable where TContext : DbContext
{
    private readonly TContext _context;

    /// <summary>  </summary>
    public EntityFrameworkCoreOutboxMailRepository(IDbContextFactory<TContext> dbContextFactory)
    {
        _context = dbContextFactory.CreateDbContext();
    }

    /// <summary>  </summary>
    public Task<List<OutboxMail>> GetPendingMails(CancellationToken cancellationToken)
    {
        return _context.Set<OutboxMail>().Where(x => x.Pending).ToListAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public async Task Add(OutboxMail outboxMail, CancellationToken cancellationToken)
    {
        await _context.Set<OutboxMail>().AddAsync(outboxMail, cancellationToken).AsTask();
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task Update(OutboxMail outboxMail, CancellationToken cancellationToken)
    {
        _context.Set<OutboxMail>().Update(outboxMail);
        return _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public void Dispose()
    {
        if (_context is IDisposable contextDisposable)
            contextDisposable.Dispose();
        else
            _ = _context.DisposeAsync().AsTask();
    }

    /// <summary>  </summary>
    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}