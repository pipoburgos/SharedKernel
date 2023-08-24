namespace SharedKernel.Domain.Specifications
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectContainsOrEqualSpecification<T> : Specification<T> where T : class
    {
        private string Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public ObjectContainsOrEqualSpecification(string value)
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

            var properties = typeof(T).GetProperties().Where(p => p.CanWrite && p.PropertyType == typeof(string));

            foreach (var property in properties)
            {
                specification = specification.Or(new PropertyContainsOrEqualSpecification<T>(property, Value));
            }

            return specification.SatisfiedBy();
        }
    }
}
