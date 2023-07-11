namespace SharedKernel.Application.Cqrs.Queries.Entities
{
    /// <summary> Filter by a property. </summary>
    public class FilterProperty
    {
        /// <summary>  </summary>
        public FilterProperty(string field, string value, FilterOperator? @operator = default, bool ignoreCase = true)
        {
            Field = field;
            Value = value;
            Operator = @operator;
            IgnoreCase = ignoreCase;
        }

        /// <summary> The data item field to which the filter operator is applied. </summary>
        public string Field { get; }

        /// <summary> The value to which the field is compared. Has to be of the same type as the field. </summary>
        public string Value { get; }

        /// <summary> The filter operator (comparison). </summary>
        public FilterOperator? Operator { get; }

        /// <summary> Determines if the string comparison is case-insensitive. </summary>
        public bool? IgnoreCase { get; set; }
    }
}
