using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Data.Mongo;
using SharedKernel.Infrastructure.Data.Mongo.Repositories;
using System;
using System.Linq;

namespace SharedKernel.Integration.Tests.Data.Mongo.Repositories
{
    internal class UserMongoRepository : MongoRepository<User, Guid>
    {
        static UserMongoRepository()
        {
            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.AutoMap();
                cm.MapCreator(p => new User(p.Id, p.Name, p.Emails.ToList(), p.Addresses.ToList()));
            });
        }

        public UserMongoRepository(IOptions<MongoSettings> mongoSettings) : base(mongoSettings)
        {
        }
    }
}
