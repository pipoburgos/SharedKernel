using System;
using System.Linq;
using System.Linq.Expressions;
using SharedKernel.Domain.Specifications.Common;

namespace SharedKernel.Domain.Specifications
{
    public class ObjectContainsOrEqualSpecification<T> : Specification<T> where T : class
    {
        private string Value { get; }

        public ObjectContainsOrEqualSpecification(string value)
        {
            Value = value;
        }

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