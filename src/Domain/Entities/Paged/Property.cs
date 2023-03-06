namespace SharedKernel.Domain.Entities.Paged
{
    /// <summary>  </summary>
    public class Property
    {
        /// <summary>  </summary>
        public Property(string field, string value, Operator? @operator, bool ignoreCase)
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
        public Operator? Operator { get; }

        /// <summary> Determines if the string comparison is case-insensitive. </summary>
        public bool IgnoreCase { get; set; }
    }
}