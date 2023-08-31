using Microsoft.Extensions.Configuration;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Data.FileSystem.Repositories;

/// <summary>  </summary>
public abstract class FileSystemRepositoryAsync<TAggregateRoot, TId> : FileSystemRepository<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot, IEntity<TId>
{
    /// <summary>  </summary>
    protected FileSystemRepositoryAsync(UnitOfWorkAsync unitOfWorkAsync, IConfiguration configuration,
        IJsonSerializer jsonSerializer) : base(unitOfWorkAsync, configuration, jsonSerializer)
    {
    }
}
