namespace SharedKernel.Application.Cqrs.Queries.Entities
{
    public class ListItemBase<T>
    {
        public bool Deleted { get; set; }

        public T Id { get; set; }
    }
}
