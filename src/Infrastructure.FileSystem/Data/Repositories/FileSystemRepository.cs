using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using SharedKernel.Domain.Specifications;
using SharedKernel.Infrastructure.Data.Repositories;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.FileSystem.Data.Repositories;

/// <summary>  </summary>
public class FileSystemRepository<TAggregateRoot, TId> : SaveRepository, IRepository<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot<TId> where TId : notnull
{
    /// <summary>  </summary>
    protected readonly IJsonSerializer JsonSerializer;

    /// <summary>  </summary>
    protected readonly string Directory;

    /// <summary>  </summary>
    protected FileSystemRepository(UnitOfWork unitOfWork, IConfiguration configuration, IJsonSerializer jsonSerializer) : base(unitOfWork)
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
    public void Add(TAggregateRoot aggregateRoot)
    {
        UnitOfWork.AddOperation(aggregateRoot,
            () => File.WriteAllText(FileName(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot)));
    }

    /// <summary>  </summary>
    public void AddRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Add(aggregateRoot);
        }
    }

    /// <summary>  </summary>
    public TAggregateRoot? GetById(TId id)
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
    public bool Any(TId id)
    {
        return GetById(id) != default;
    }

    /// <summary>  </summary>
    public bool NotAny(TId id)
    {
        return GetById(id) == default;
    }

    /// <summary>  </summary>
    public void Update(TAggregateRoot aggregateRoot)
    {
        UnitOfWork.UpdateOperation(aggregateRoot,
            () => File.WriteAllText(FileName(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot)));
    }

    /// <summary>  </summary>
    public void UpdateRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Update(aggregateRoot);
        }
    }

    /// <summary>  </summary>
    public void Remove(TAggregateRoot aggregateRoot)
    {
        UnitOfWork.RemoveOperation(aggregateRoot, () => File.Delete(FileName(aggregateRoot.Id)),
            () => File.WriteAllText(FileName(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot)));
    }

    /// <summary>  </summary>
    public void RemoveRange(IEnumerable<TAggregateRoot> aggregates)
    {
        foreach (var aggregateRoot in aggregates)
        {
            Remove(aggregateRoot);
        }
    }
}
