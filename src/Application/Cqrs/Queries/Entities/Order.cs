namespace SharedKernel.Application.Cqrs.Queries.Entities
{
    public class Order
    {
        public Order(string field, bool ascending)
        {
            Field = field;
            Ascending = ascending;
        }

        public string Field { get;}

        public bool Ascending { get; }
    }
}