namespace SharedKernel.Application.Cqrs.Queries.Entities
{
    /// <summary>
    /// Filter by a property
    /// </summary>
    public class FilterProperty
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public FilterProperty(string field, string value)
        {
            Field = field;
            Value = value;
        }

        /// <summary>
        /// The data item field to which the filter operator is applied.
        /// </summary>
        public string Field { get; }

        /// <summary>
        /// The value to which the field is compared. Has to be of the same type as the field.
        /// </summary>
        public string Value { get; }
    }
}
