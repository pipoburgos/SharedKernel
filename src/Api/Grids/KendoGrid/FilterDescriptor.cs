namespace SharedKernel.Api.Grids.KendoGrid
{
    /// <summary>
    /// 
    /// </summary>
    public class FilterDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="operator"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        public FilterDescriptor(string field, string @operator, string value, bool ignoreCase)
        {
            Field = field;
            Operator = @operator;
            Value = value;
            IgnoreCase = ignoreCase;
        }

        /**
         * The data item field to which the filter operator is applied.
         */
        public string Field { get; }

        /**
         * The filter operator (comparison).
         *
         * The supported operators are:
         * * `"eq"` (equal to)
         * * `"neq"` (not equal to)
         * * `"isnull"` (is equal to null)
         * * `"isnotnull"` (is not equal to null)
         * * `"lt"` (less than)
         * * `"lte"` (less than or equal to)
         * * `"gt"` (greater than)
         * * `"gte"` (greater than or equal to)
         *
         * The following operators are supported for string fields only:
         * * `"startswith"`
         * * `"endswith"`
         * * `"contains"`
         * * `"doesnotcontain"`
         * * `"isempty"`
         * * `"isnotempty"`
         */
        public string Operator { get; }

        /**
         * The value to which the field is compared. Has to be of the same type as the field.
         */
        public string Value { get; }

        /**
         * Determines if the string comparison is case-insensitive.
         */
        public bool IgnoreCase { get; }
    }
}
