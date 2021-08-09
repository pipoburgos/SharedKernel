using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq;

namespace SharedKernel.Infrastructure.Data.Mongo.Queries
{
    /// <summary>
    /// Query provider for Mongo database
    /// </summary>
    public sealed class MongoQueryProvider
    {
        private readonly IMongoDatabase _mongoDatabase;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mongoSettings"></param>
        public MongoQueryProvider(IOptions<MongoSettings> mongoSettings)
        {
            _mongoDatabase = new MongoClient(mongoSettings.Value.ConnectionString)
                .GetDatabase(mongoSettings.Value.Database);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> GetQuery<T>()
        {
            return _mongoDatabase.GetCollection<T>(typeof(T).Name).AsQueryable();
        }
    }
}
