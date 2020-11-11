namespace SharedKernel.Application.Cqrs.Queries.Entities
{
    public class ComboDto<TKey>
    {
        public TKey Value { get; set; }

        public string Text { get; set; }
    }
}