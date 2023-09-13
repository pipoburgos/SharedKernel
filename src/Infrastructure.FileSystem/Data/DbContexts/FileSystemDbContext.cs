using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.Data.Services;

namespace SharedKernel.Infrastructure.FileSystem.Data.DbContexts;

/// <summary>  </summary>
public abstract class FileSystemDbContext : DbContextAsync
{
    /// <summary>  </summary>
    protected readonly IJsonSerializer JsonSerializer;

    /// <summary>  </summary>
    protected readonly string Directory;

    /// <summary>  </summary>
    protected string FileName<TAggregateRoot, TId>(TId id) => $"{Directory}/{typeof(TAggregateRoot).Name}.{id}.json";

    /// <summary>  </summary>
    protected FileSystemDbContext(IConfiguration configuration, IJsonSerializer jsonSerializer,
        IEntityAuditableService auditableService, IClassValidatorService classValidatorService) : base(auditableService,
        classValidatorService)
    {
        JsonSerializer = jsonSerializer;
        Directory = configuration.GetSection("FileSystemRepositoryPath").Value ?? AppDomain.CurrentDomain.BaseDirectory;

        if (string.IsNullOrWhiteSpace(Directory))
            throw new Exception("Empty FileSystemRepositoryPath key on appsettings.");
    }

    /// <summary>  </summary>
    protected override void AddMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        File.WriteAllText(FileName<TAggregateRoot, TId>(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot));
    }

    /// <summary>  </summary>
    protected override void UpdateMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        File.WriteAllText(FileName<TAggregateRoot, TId>(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot));
    }

    /// <summary>  </summary>
    protected override void DeleteMethod<TAggregateRoot, TId>(TAggregateRoot aggregateRoot)
    {
        File.Delete(FileName<TAggregateRoot, TId>(aggregateRoot.Id));
    }

    /// <summary>  </summary>
    public override TAggregateRoot? GetById<TAggregateRoot, TId>(TId id) where TAggregateRoot : class
    {
        if (!File.Exists(FileName<TAggregateRoot, TId>(id)))
            return default;

        var text = File.ReadAllText(FileName<TAggregateRoot, TId>(id));
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
    protected override Task AddMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        return File.WriteAllTextAsync(FileName<TAggregateRoot, TId>(aggregateRoot.Id),
            JsonSerializer.Serialize(aggregateRoot), cancellationToken);
#else
        File.WriteAllText(FileName<TAggregateRoot, TId>(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot));
        return Task.CompletedTask;
#endif
    }

    /// <summary>  </summary>
    protected override Task UpdateMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        return File.WriteAllTextAsync(FileName<TAggregateRoot, TId>(aggregateRoot.Id),
            JsonSerializer.Serialize(aggregateRoot), cancellationToken);
#else
        File.WriteAllText(FileName<TAggregateRoot, TId>(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot));
        return Task.CompletedTask;
#endif
    }

    /// <summary>  </summary>
    protected override Task DeleteMethodAsync<TAggregateRoot, TId>(TAggregateRoot aggregateRoot,
        CancellationToken cancellationToken)
    {
        File.Delete(FileName<TAggregateRoot, TId>(aggregateRoot.Id));
        return Task.CompletedTask;
    }

    /// <summary>  </summary>
    public override async Task<TAggregateRoot?> GetByIdAsync<TAggregateRoot, TId>(TId id,
        CancellationToken cancellationToken) where TAggregateRoot : class

    {
        if (!File.Exists(FileName<TAggregateRoot, TId>(id)))
            return default;

#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        var text = await File.ReadAllTextAsync(FileName<TAggregateRoot, TId>(id), cancellationToken);
#else
        var text = File.ReadAllText(FileName<TAggregateRoot, TId>(id));
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
