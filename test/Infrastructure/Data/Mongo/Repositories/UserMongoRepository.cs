using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Data.Mongo;
using SharedKernel.Infrastructure.Data.Mongo.Repositories;
using System;

namespace SharedKernel.Integration.Tests.Data.Mongo.Repositories
{
    internal class UserMongoRepository : MongoRepository<User, Guid>
    {
        static UserMongoRepository()
        {
            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.AutoMap();
                cm.MapField("_emails");
                cm.MapField("_addresses");
            });
        }

        public UserMongoRepository(IOptions<MongoSettings> mongoSettings) : base(mongoSettings)
        {
        }
    }
}
