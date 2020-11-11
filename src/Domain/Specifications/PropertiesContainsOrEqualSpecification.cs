using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharedKernel.Domain.Entities.Paged;
using SharedKernel.Domain.Specifications.Common;

namespace SharedKernel.Domain.Specifications
{
    public class PropertiesContainsOrEqualSpecification<T> : ISpecification<T> where T : class
    {
        private readonly IEnumerable<Property> _properties;

        public PropertiesContainsOrEqualSpecification(IEnumerable<Property> properties)
        {
            _properties = properties;
        }

        public Expression<Func<T, bool>> SatisfiedBy()
        {
            ISpecification<T> filter = new TrueSpecification<T>();

            if (_properties == null)
                return filter.SatisfiedBy();

            foreach (var property in _properties.Where(f => !string.IsNullOrWhiteSpace(f.Value)))
            {
                var propertyInfo = typeof(T).GetProperties().Where(p => p.CanWrite).SingleOrDefault(t => t.Name.ToUpper() == property.Field.ToUpper());

                if(propertyInfo == null)
                    throw new ArgumentNullException(nameof(propertyInfo));

                filter = filter.And(new PropertyContainsOrEqualSpecification<T>(propertyInfo, property.Value));
            }


            return filter.SatisfiedBy();
        }
    }
}
