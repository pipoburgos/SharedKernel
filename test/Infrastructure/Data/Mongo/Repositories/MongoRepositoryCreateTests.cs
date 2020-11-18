using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Data.Mongo;
using SharedKernel.Integration.Tests.Shared;
using System;
using Microsoft.Extensions.Options;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.Mongo.Repositories
{
    public class MongoRepositoryCreateTests : InfrastructureTestCase
    {
        protected override string GetJsonFile()
        {
            return "Data/Mongo/appsettings.mongo.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services.Configure<MongoSettings>(Configuration.GetSection(nameof(MongoSettings)));
        }

        [Fact]
        public void AddOk()
        {
            var mongoSettings = GetRequiredService<IOptions<MongoSettings>>();

            var repository = new UserMongoRepository(mongoSettings);

            var roberto = User.Create(Guid.NewGuid(), "Roberto bbdd");
            repository.Add(roberto);

            repository.SaveChanges();

            Assert.Equal(roberto, repository.GetById(roberto.Id));
        }
    }
}
