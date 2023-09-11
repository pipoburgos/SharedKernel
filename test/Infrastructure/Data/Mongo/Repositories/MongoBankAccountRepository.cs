using MongoDB.Bson.Serialization;
using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Infrastructure.Mongo.Data.Repositories;
using SharedKernel.Integration.Tests.Data.Mongo.DbContexts;

namespace SharedKernel.Integration.Tests.Data.Mongo.Repositories;

public class MongoBankAccountRepository : MongoRepository<BankAccount, Guid>
{
    static MongoBankAccountRepository()
    {
        BsonClassMap.RegisterClassMap<BankAccount>(cm =>
        {
            cm.AutoMap();
        });
    }

    public MongoBankAccountRepository(SharedKernelMongoDbContext mongoUnitOfWork) : base(mongoUnitOfWork)
    {
    }
}
