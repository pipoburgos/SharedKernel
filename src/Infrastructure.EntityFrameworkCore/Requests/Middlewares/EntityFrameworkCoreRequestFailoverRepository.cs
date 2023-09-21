using SharedKernel.Infrastructure.Requests.Middlewares.Failover;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Requests.Middlewares;

/// <summary>  </summary>
public class EntityFrameworkCoreRequestFailoverRepository<TContext> : IRequestFailoverRepository where TContext : DbContext
{
    private readonly TContext _context;

    /// <summary>  </summary>
    public EntityFrameworkCoreRequestFailoverRepository(TContext context)
    {
        _context = context;
    }

    /// <summary>  </summary>
    public async Task Save(ErrorRequest request, CancellationToken cancellationToken)
    {
        await _context.Set<ErrorRequest>().AddAsync(request, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
