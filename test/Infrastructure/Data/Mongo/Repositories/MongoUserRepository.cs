using MongoDB.Bson.Serialization;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Mongo.Data.Repositories;

namespace SharedKernel.Integration.Tests.Data.Mongo.Repositories;

public class MongoUserRepository : MongoRepository<User, Guid>
{
    static MongoUserRepository()
    {
        BsonClassMap.RegisterClassMap<User>(cm =>
        {
            cm.AutoMap();
            cm.MapField("_emails");
            cm.MapField("_addresses");
        });
    }

    public MongoUserRepository(SharedKernelMongoUnitOfWork mongoUnitOfWork) : base(mongoUnitOfWork)
    {
    }
}
