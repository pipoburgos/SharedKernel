using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.RailwayOrientedProgramming;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Repositories.Save;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.FileSystem.Data.Repositories;

/// <summary>  </summary>
public abstract class FileSystemRepositoryAsync<TAggregateRoot, TId> : FileSystemRepository<TAggregateRoot, TId>,
    IRepositoryAsync<TAggregateRoot, TId>,
    ISaveRepositoryAsync where TAggregateRoot : class, IAggregateRoot, IEntity<TId> where TId : notnull
{
    private readonly UnitOfWorkAsync _unitOfWorkAsync;

    /// <summary>  </summary>
    protected FileSystemRepositoryAsync(UnitOfWorkAsync unitOfWorkAsync, IConfiguration configuration,
        IJsonSerializer jsonSerializer) : base(unitOfWorkAsync, configuration, jsonSerializer)
    {
        _unitOfWorkAsync = unitOfWorkAsync;
    }

    /// <summary>  </summary>
    public Task AddAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        return _unitOfWorkAsync.AddOperationAsync(aggregateRoot,
            () => File.WriteAllTextAsync(FileName(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot), cancellationToken));
#else
        return _unitOfWorkAsync.AddOperationAsync(aggregateRoot, () =>
        {
            File.WriteAllText(FileName(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot));
            return Task.CompletedTask;
        });
#endif
    }

    /// <summary>  </summary>
    public Task AddRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return Task.WhenAll(aggregates.Select(a => AddAsync(a, cancellationToken)));
    }

    /// <summary>  </summary>
    public async Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        if (!File.Exists(FileName(id)))
            return default;
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        var text = await File.ReadAllTextAsync(FileName(id), cancellationToken);
#else
        var text = File.ReadAllText(FileName(id));
        await Task.CompletedTask;
#endif
        var aggregateRoot = JsonSerializer.Deserialize<TAggregateRoot>(text);
        if (aggregateRoot is IEntityAuditableLogicalRemove a)
        {
            return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(a) ? default : aggregateRoot;
        }

        return aggregateRoot;
    }

    /// <summary>  </summary>
    public async Task<bool> AnyAsync(TId id, CancellationToken cancellationToken)
    {
        return await GetByIdAsync(id, cancellationToken) != default;
    }

    /// <summary>  </summary>
    public async Task<bool> NotAnyAsync(TId id, CancellationToken cancellationToken)
    {
        return await GetByIdAsync(id, cancellationToken) == default;
    }

    /// <summary>  </summary>
    public Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        return _unitOfWorkAsync.UpdateOperationAsync(aggregateRoot,
            () => File.WriteAllTextAsync(FileName(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot), cancellationToken));
#else
        return _unitOfWorkAsync.UpdateOperationAsync(aggregateRoot, () =>
        {
            File.WriteAllText(FileName(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot));
            return Task.CompletedTask;
        });
#endif
    }

    /// <summary>  </summary>
    public Task UpdateRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return Task.WhenAll(aggregates.Select(a => UpdateAsync(a, cancellationToken)));
    }

    /// <summary>  </summary>
    public Task RemoveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
    {
        return _unitOfWorkAsync.RemoveOperationAsync(aggregateRoot, () =>
        {
            File.Delete(FileName(aggregateRoot.Id));
            return Task.CompletedTask;
        }, () =>
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
                File.WriteAllTextAsync(FileName(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot), cancellationToken)
#else
            {
                File.WriteAllText(FileName(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot));
                return Task.CompletedTask;
            }
#endif
        );
    }

    /// <summary>  </summary>
    public Task RemoveRangeAsync(IEnumerable<TAggregateRoot> aggregates, CancellationToken cancellationToken)
    {
        return Task.WhenAll(aggregates.Select(a => RemoveAsync(a, cancellationToken)));
    }

    /// <summary>  </summary>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _unitOfWorkAsync.SaveChangesAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken)
    {
        return _unitOfWorkAsync.SaveChangesResultAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        return _unitOfWorkAsync.RollbackAsync(cancellationToken);
    }

    /// <summary>  </summary>
    public Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken)
    {
        return _unitOfWorkAsync.RollbackResultAsync(cancellationToken);
    }
}
