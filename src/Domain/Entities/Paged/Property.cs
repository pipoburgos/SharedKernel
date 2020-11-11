namespace SharedKernel.Domain.Entities.Paged
{
    public class Property
    {
        public Property(string field, string value)
        {
            Field = field;
            Value = value;
        }

        /**
         * The data item field to which the filter operator is applied.
         * */
        public string Field { get; }

        /**
         * The value to which the field is compared. Has to be of the same type as the field.
         */
        public string Value { get; }
    }
}