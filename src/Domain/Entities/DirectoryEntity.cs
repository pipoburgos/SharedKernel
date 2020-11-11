using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Entities
{
    public class DirectoryEntity : AggregateRoot<string>
    {
        protected DirectoryEntity() { }

        public static DirectoryEntity Create(string path)
        {
            return new DirectoryEntity
            {
                Id = path
            };
        }


        public string Name { get; set; }
    }
}
