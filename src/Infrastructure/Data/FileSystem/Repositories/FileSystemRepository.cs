using SharedKernel.Application.Serializers;

#pragma warning disable 693

namespace SharedKernel.Infrastructure.Data.FileSystem.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class FileSystemRepository<TAggregateRoot, TKey> :
        ICreateRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot, IEntity<TKey>
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly IJsonSerializer JsonSerializer;

        /// <summary>
        /// 
        /// </summary>
        protected readonly string FilePath;

        /// <summary>
        /// 
        /// </summary>
        protected FileSystemRepository(IJsonSerializer jsonSerializer)
        {
            JsonSerializer = jsonSerializer;
            FilePath = Directory.GetCurrentDirectory() + typeof(TAggregateRoot);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string FileName(string id)
        {
            return $"{FilePath}.{id}.repository";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        public void Add(TAggregateRoot aggregate)
        {
            // ReSharper disable once RedundantSuppressNullableWarningExpression
            using var outputFile = new StreamWriter(FileName(aggregate.Id!.ToString()!), false);
            outputFile.WriteLine(JsonSerializer.Serialize(aggregate));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        public void AddRange(IEnumerable<TAggregateRoot> aggregates)
        {
            foreach (var aggregateRoot in aggregates)
            {
                Add(aggregateRoot);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public TAggregateRoot? GetById<TKey>(TKey key) where TKey : notnull
        {
            // ReSharper disable once RedundantSuppressNullableWarningExpression
            var id = key.ToString()!;
            if (!File.Exists(FileName(id)))
                return default;

            var text = File.ReadAllText(FileName(id));
            return JsonSerializer.Deserialize<TAggregateRoot>(text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Rollback()
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return 0;
        }
    }
}
