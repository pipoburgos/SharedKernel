using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Data.Repositories;
using SharedKernel.Infrastructure.FileSystem.Data.DbContexts;

namespace SharedKernel.Infrastructure.FileSystem.Data.Repositories;

/// <summary>  </summary>
public class FileSystemRepository<TAggregateRoot, TId> : RepositoryAsync<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
{
    /// <summary>  </summary>
    protected readonly IJsonSerializer JsonSerializer;

    /// <summary>  </summary>
    protected readonly string Directory;

    /// <summary>  </summary>
    protected FileSystemRepository(FileSystemDbContext fileSystemDbContext, IConfiguration configuration,
        IJsonSerializer jsonSerializer) : base(fileSystemDbContext)
    {
        JsonSerializer = jsonSerializer;
        Directory = configuration.GetSection("FileSystemRepositoryPath").Value ?? AppDomain.CurrentDomain.BaseDirectory;

        if (string.IsNullOrWhiteSpace(Directory))
            throw new Exception("Empty FileSystemRepositoryPath key on appsettings.");
    }

    /// <summary>  </summary>
    protected string FileName(TId id)
    {
        return $"{Directory}/{typeof(TAggregateRoot).Name}.{id}.json";
    }

    /// <summary>  </summary>
    public override TAggregateRoot? GetById(TId id)
    {
        if (!File.Exists(FileName(id)))
            return default;

        var text = File.ReadAllText(FileName(id));
        var aggregateRoot = JsonSerializer.Deserialize<TAggregateRoot>(text);
        if (aggregateRoot is IEntityAuditableLogicalRemove a)
        {
            return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(a)
                ? default
                : aggregateRoot;
        }

        return aggregateRoot;
    }

    /// <summary>  </summary>
    public override async Task<TAggregateRoot?> GetByIdAsync(TId id, CancellationToken cancellationToken)
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
            return new DeletedSpecification<IEntityAuditableLogicalRemove>().SatisfiedBy().Compile()(a)
                ? default
                : aggregateRoot;
        }

        return aggregateRoot;
    }
}
