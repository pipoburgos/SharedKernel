namespace SharedKernel.Domain.Entities.Paged
{
    /// <summary>
    /// 
    /// </summary>
    public class Property
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public Property(string field, string value)
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