using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Data.Mongo;
using SharedKernel.Testing.Infrastructure;
using System.Linq;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.Mongo.Repositories
{
    [Collection("DockerHook")]
    public class MongoRepositoryCreateTests : InfrastructureTestCase<FakeStartup>
    {
        protected override string GetJsonFile()
        {
            return "Data/Mongo/appsettings.mongo.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services.AddMongo(Configuration);
        }

        [Fact]
        public void AddOk()
        {
            var mongoSettings = GetRequiredService<IOptions<MongoSettings>>();

            var repository = new UserMongoRepository(mongoSettings);

            var roberto = UserMother.Create();
            repository.Add(roberto);

            repository.SaveChanges();

            Assert.Equal(roberto, repository.GetById(roberto.Id));
        }

        [Fact]
        public void SaveRepositoryNameChanged()
        {
            var repository = new UserMongoRepository(GetRequiredService<IOptions<MongoSettings>>());

            var roberto = UserMother.Create();

            for (var i = 0; i < 10; i++)
            {
                roberto.AddAddress(AddressMother.Create());
            }

            for (var i = 0; i < 5; i++)
            {
                roberto.AddEmail(EmailMother.Create());
            }

            repository.Add(roberto);

            repository.SaveChanges();

            var repository2 = new UserMongoRepository(GetRequiredService<IOptions<MongoSettings>>());
            var repoUser = repository2.GetById(roberto.Id);

            Assert.Equal(roberto.Id, repoUser.Id);
            Assert.Equal(roberto.Name, repoUser.Name);
            Assert.Equal(5, repoUser.Emails.Count());
            Assert.Equal(10, repoUser.Addresses.Count());
        }
    }
}
