using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.DbContexts;
using SharedKernel.Infrastructure.Data.Services;

namespace SharedKernel.Infrastructure.FileSystem.Data.DbContexts;

/// <summary>  </summary>
public abstract class FileSystemDbContext : DbContext
{
    /// <summary>  </summary>
    protected readonly IJsonSerializer JsonSerializer;

    /// <summary>  </summary>
    protected readonly string Directory;

    /// <summary>  </summary>
    protected string FileName<T, TId>(TId id) => $"{Directory}/{typeof(T).Name}.{id}.json";

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
    protected override void AddMethod<T, TId>(T aggregateRoot)
    {
        File.WriteAllText(FileName<T, TId>(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot));
    }

    /// <summary>  </summary>
    protected override void UpdateMethod<T, TId>(T aggregateRoot)
    {
        File.WriteAllText(FileName<T, TId>(aggregateRoot.Id), JsonSerializer.Serialize(aggregateRoot));
    }

    /// <summary>  </summary>
    protected override void DeleteMethod<T, TId>(T aggregateRoot)
    {
        File.Delete(FileName<T, TId>(aggregateRoot.Id));
    }
}
