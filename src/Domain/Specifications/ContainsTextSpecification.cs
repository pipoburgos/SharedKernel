namespace SharedKernel.Domain.Specifications
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ContainsTextSpecification<T> : Specification<T>
    {
        private string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public ContainsTextSpecification(string value)
        {
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            ISpecification<T> specification = new FalseSpecification<T>();

            specification = typeof(T) == typeof(string)
                ? specification.Or(new TheComparisonMatchesSpecification<T>(default, Value, Operator.Contains))
                : typeof(T)
                    .GetProperties()
                    .Where(p => p.CanWrite && p.PropertyType == typeof(string))
                    .Aggregate(specification, (current, property) =>
                        current.Or(new TheComparisonMatchesSpecification<T>(property, Value, Operator.Contains)));

            return specification.SatisfiedBy();
        }
    }
}