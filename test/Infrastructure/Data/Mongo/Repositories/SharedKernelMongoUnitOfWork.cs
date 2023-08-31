using Microsoft.Extensions.Options;
using SharedKernel.Infrastructure.Mongo.Data;
using SharedKernel.Infrastructure.Mongo.Data.UnitOfWorks;

namespace SharedKernel.Integration.Tests.Data.Mongo.Repositories;

public class SharedKernelMongoUnitOfWork : MongoUnitOfWorkAsync, ISharedKernelUnitOfWork
{
    public SharedKernelMongoUnitOfWork(IOptions<MongoSettings> options) : base(options)
    {
    }
}