using SharedKernel.Domain.Entities.Paged;
using SharedKernel.Domain.Specifications.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SharedKernel.Domain.Specifications
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AllPropertiesValueMatchesSpecification<T> : ISpecification<T>
    {
        private readonly IEnumerable<Property> _properties;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        public AllPropertiesValueMatchesSpecification(IEnumerable<Property> properties)
        {
            _properties = properties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Expression<Func<T, bool>> SatisfiedBy()
        {
            ISpecification<T> filter = new TrueSpecification<T>();

            if (_properties == null)
                return filter.SatisfiedBy();

            if (!new IsClassTypeSpecification<T>().SatisfiedBy().Compile()(default))
            {
                var property = _properties.SingleOrDefault();
                if (property != default)
                    filter = filter.And(new TheComparisonMatchesSpecification<T>(default, property.Value, property.Operator));

                return filter.SatisfiedBy();
            }

            foreach (var property in _properties)
            {
                var propertyInfo = typeof(T)
                    .GetProperties()
                    .Where(p => p.CanRead)
                    .SingleOrDefault(t => t.Name.ToUpper() == property.Field.ToUpper());

                if (propertyInfo == null)
                    throw new ArgumentNullException(nameof(propertyInfo));

                filter = filter.And(new TheComparisonMatchesSpecification<T>(propertyInfo, property.Value, property.Operator));
            }

            return filter.SatisfiedBy();
        }
    }
}
