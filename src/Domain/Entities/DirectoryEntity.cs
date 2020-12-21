using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class DirectoryEntity : AggregateRoot<string>
    {
        /// <summary>
        /// 
        /// </summary>
        protected DirectoryEntity() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DirectoryEntity Create(string path)
        {
            return new DirectoryEntity
            {
                Id = path
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}
