using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Data.Mongo;
using SharedKernel.Infrastructure.Data.Mongo.Repositories;
using System;
using Microsoft.Extensions.Options;

namespace SharedKernel.Integration.Tests.Data.Mongo.Repositories
{
    internal class UserMongoRepository : MongoRepository<User, Guid>
    {
        public UserMongoRepository(IOptions<MongoSettings> mongoSettings) : base(mongoSettings)
        {
        }
    }
}
