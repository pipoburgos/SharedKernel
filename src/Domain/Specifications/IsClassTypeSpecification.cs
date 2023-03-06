using SharedKernel.Domain.Specifications.Common;
using System;
using System.Collections;
using System.Linq.Expressions;

namespace SharedKernel.Domain.Specifications
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IsClassTypeSpecification<T> : ISpecification<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Expression<Func<T, bool>> SatisfiedBy()
        {
            return x =>
                typeof(T).IsClass &&
                typeof(T) != typeof(string) &&
                !typeof(IEnumerable).IsAssignableFrom(typeof(T));
        }
    }
}
