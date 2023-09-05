using MongoDB.Bson.Serialization;
using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Infrastructure.Mongo.Data.Repositories;

namespace SharedKernel.Integration.Tests.Data.Mongo.Repositories;

public class MongoBankAccountRepository : MongoRepositoryAsync<BankAccount, Guid>
{
    static MongoBankAccountRepository()
    {
        BsonClassMap.RegisterClassMap<BankAccount>(cm =>
        {
            cm.AutoMap();
        });
    }

    public MongoBankAccountRepository(SharedKernelMongoUnitOfWork mongoUnitOfWork) : base(mongoUnitOfWork)
    {
    }
}
