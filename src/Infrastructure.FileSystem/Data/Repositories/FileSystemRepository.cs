using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
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
}
